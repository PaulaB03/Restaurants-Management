import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';

interface Order {
  orderId?: number;
  userId: string;
  restaurantId: number;
  orderDate?: Date;
  restaurantName?: string;
  orderItems: OrderItem[];
  totalPrice?: number;
}

interface OrderItem {
  id: number;
  productId: number;
  productName?: string; 
  productPrice?: number;
  quantity: number;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  userDetails: any;
  userId: string | null = null;
  orders: Order[] = [];

  constructor(private authService: AuthService, private apiService: ApiService) {}

  ngOnInit(): void {
    this.authService.getUserByEmail().subscribe({
      next: user => {
        this.userDetails = user;
        this.userId = user.id;
        this.getUserOrders();
      },
      error: error => {
        console.error('Failed to fetch user details', error);
      }
    });
  }

  getUserOrders(): void {
    this.apiService.getOrders(this.userId!).subscribe({
      next: (orders: Order[]) => {
        this.orders = orders.sort((a, b) => {
          return new Date(b.orderDate!).getTime() - new Date(a.orderDate!).getTime();
        });
        
        this.orders.forEach(order => {
          // Fetch restaurant details
          this.apiService.getRestaurantById(order.restaurantId).subscribe({
            next: restaurant => {
              order.restaurantName = restaurant.name;
            },
            error: error => {
              console.error('Error fetching restaurant details:', error);
            }
          });
          
          // Fetch product details for each order item
          order.orderItems.forEach(item => {
            this.apiService.getProductById(item.productId).subscribe({
              next: product => {
                item.productName = product.name;
                item.productPrice = product.price;
              },
              error: error => {
                console.error('Error fetching product details:', error);
              }
            });
          });
        });
      },
      error: (error) => {
        console.error('Error fetching user orders:', error);
      }
    });
  }

  deleteOrder(orderId: number): void {
    this.apiService.deleteOrder(orderId).subscribe({
      next: () => {
        this.orders = this.orders.filter(order => order.orderId !== orderId);
      },
      error: (error) => {
        console.error('Error deleting order:', error);
      }
    });
  }
}
