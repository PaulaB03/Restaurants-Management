import { Component, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { FormsModule } from '@angular/forms';

interface Product {
  id?
  : number;
  name: string;
  price: number;
  restaurantId: number;
  categoryId: number;
}

interface Category {
  id: number;
  name: string;
  products: Product[];
}

interface Order {
  orderId?: number;
  userId: string;
  restaurantId: number;
  orderItems: OrderItem[];
}

interface OrderItem {
  id?: number;
  productId: number;
  productName?: string;
  productPrice?: number;
  quantity: number;
}

@Component({
  selector: 'app-restaurant',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './restaurant.component.html',
  styleUrl: './restaurant.component.css'
})
export class RestaurantComponent {
  restaurant: any;
  categories: Category[] = [];
  allCategories: Category[] = [];
  isOwner: boolean = false;
  userId: string | null = null;
  editProduct: Product | null = null;
  cart: OrderItem[] = []; 
  newOrder: Order | null = null;
  activeCart: boolean = false;
  activateAddForm: boolean = false;

  newProduct: Product = {
    name: '',
    price: 0,
    restaurantId: 0, 
    categoryId: 0
  };

  constructor(private route: ActivatedRoute, private router: Router, private apiService: ApiService, private authService: AuthService, private renderer: Renderer2) {}

  ngOnInit(): void {
    this.authService.getUserByEmail().subscribe({
      next: user => {
        this.userId = user.id;
      },
      error: error => {
        console.error('Failed to fetch user details', error);
      }
    });

    this.isOwner = this.authService.hasRole('Owner');
    const id = +this.route.snapshot.paramMap.get('id')!; 
    this.getRestaurant(id);
    this.getCategoriesWithProducts(id);
    this.getAllCategories();
    this.newProduct.restaurantId = id;
  }

  getRestaurant(restaurantId: number): void {
    this.apiService.getRestaurantById(restaurantId).subscribe({
      next: (res) => {
        console.log('Restaurant details:', res)
        this.restaurant = res;
      },
      error: (error) => console.error('Error fetching restaurant details:', error)
    });
  }

  getCategoriesWithProducts(restaurantId: number): void {
    this.apiService.getProductsByRestaurant(restaurantId).subscribe({
      next: (products) => {
        this.apiService.getCategories().subscribe({
          next: (categories) => {
            this.categories = categories.map((category: Category) => ({
              ...category,
              products: products.filter((product: Product) => product.categoryId === category.id)
            })).filter((category: Category) => category.products.length > 0);
          },
          error: (error) => {
            console.error('Error fetching categories:', error);
          }
        });
      },
      error: (error) => {
        console.error('Error fetching products:', error);
      }
    });
  }

  addProduct(): void {
    this.apiService.addProduct(this.newProduct).subscribe({
      next: (res) => {
        this.getCategoriesWithProducts(this.restaurant.id);
        this.resetNewProduct();
      },
      error: (error) => {
        console.error('Error adding product:', error);
      }
    });
  }

  updateProduct(): void {
    if (this.editProduct && this.editProduct.id !== undefined) {
      this.apiService.updatePrice(this.editProduct.id, this.editProduct.price).subscribe({
        next: (res) => {
          console.log('Product updated:', res);
          this.getCategoriesWithProducts(this.restaurant.id);
          this.editProduct = null;
        },
        error: (error) => {
          console.error('Error updating product:', error);
        }
      });
    }
  }

  deleteProduct(productId: number): void {
    this.apiService.deleteProduct(productId).subscribe({
      next: (res) => {
        this.getCategoriesWithProducts(this.restaurant.id);
      },
      error: (error) => {
        console.error('Error deleting product:', error);
      }
    });
  }

  getAllCategories(): void {
    this.apiService.getCategories().subscribe({
      next: (categories: Category[]) => {
        this.allCategories = categories; 
      },
      error: (error) => {
        console.error('Error fetching all categories:', error);
      }
    });
  }

  private resetNewProduct(): void {
    this.newProduct = {
      name: '',
      price: 0,
      restaurantId: this.restaurant.id, 
      categoryId: 0
    };
  }

  startEditProduct(product: Product): void {
    this.editProduct = { ...product };
  }

  addToCart(product: Product): void {
    const cartIcon = document.querySelector('.cartIcon');
      if (cartIcon) {
        this.renderer.addClass(cartIcon, 'shake');
        setTimeout(() => {
          this.renderer.removeClass(cartIcon, 'shake');
        }, 500);
      }

    const existingItem = this.cart.find(item => item.productId === product.id);
    if (existingItem) {
      existingItem.quantity++;
    } else {
      this.cart.push({
        productId: product.id!,
        productName: product.name,
        productPrice: product.price,
        quantity: 1
      });
    }
  }

  removeFromCart(productId: number): void {
    this.cart = this.cart.filter(item => item.productId !== productId);
  }

  placeOrder(): void {
    if (this.userId && this.restaurant) {
      const order: Order = {
        userId: this.userId,
        restaurantId: this.restaurant.id,
        orderItems: this.cart
      };

      this.apiService.addOrder(order).subscribe({
        next: (res) => {
          console.log('Order placed:', res);
          this.cart = []; 
          this.router.navigate(["/profile"])
        },
        error: (error) => {
          console.error('Error placing order:', error);
        }
      });
    }
  }

  getTotalPrice(): number {
    return this.cart.reduce((total, item) => total + (item.productPrice! * item.quantity), 0);
  }

  checkCart() {
    this.activeCart = !this.activeCart;

    if (this.cart.length == 0)
      this.activeCart = false;
  }

  increaseQuantity(productId: number): void {
    const item = this.cart.find(i => i.productId === productId);
    if (item) {
      item.quantity++;
    }
  }

  decreaseQuantity(productId: number): void {
    const item = this.cart.find(i => i.productId === productId);
    if (item) {
      item.quantity--;
      if (item.quantity === 0) {
        this.removeFromCart(productId);
      }
    }
  }

  activateAdd() {
    this.activateAddForm = !this.activateAddForm;
  }

  getTotalItems(): number {
    return this.cart.reduce((total, item) => total + item.quantity, 0);
  }
}
