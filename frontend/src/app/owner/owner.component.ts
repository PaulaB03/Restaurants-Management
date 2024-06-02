import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface Restaurant {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  ownerId: string;
  address: {
    street: string;
    floor?: string;
    number: string;
    city: string;
  };
}

@Component({
  selector: 'app-owner',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './owner.component.html',
  styleUrl: './owner.component.css'
})
export class OwnerComponent {
  ownerId: any;
  restaurants: any[] = [];
  restaurantForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private apiService: ApiService, private router: Router) {
    this.restaurantForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      imageUrl: [''],
      street: ['', Validators.required],
      floor: [''],
      number: ['', Validators.required],
      city: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.authService.getUserByEmail().subscribe({
      next: user => {
        this.ownerId = user.id;
        this.getRestaurants();
      },
      error: error => {
        console.error('Failed to fetch user details', error);
      }
    });
  }

  addRestaurant(): void {
    if (this.restaurantForm.valid) {
      const formValues = this.restaurantForm.value;
      const newRestaurant = {
        name: formValues.name,
        description: formValues.description,
        imageUrl: formValues.imageUrl,
        ownerId: this.ownerId,
        address: {
          street: formValues.street,
          floor: formValues.floor,
          number: formValues.number,
          city: formValues.city
        }
      };
      this.apiService.addRestaurant(newRestaurant).subscribe({
        next: (res) => {
          console.log('Restaurant added:', res);
          this.restaurantForm.reset(); 
          this.getRestaurants();
        },
        error: (error) => {
          console.error('Error adding restaurant:', error);
        }
      });
    } else {
      console.error('Form is invalid:', this.restaurantForm);
    }
  }

  deleteRestaurant(restaurantId: number): void {
    this.apiService.deleteRestaurant(restaurantId).subscribe({
      next: (res) => {
        console.log(res);
        this.getRestaurants();
      },
      error: (error) => console.error('Error deleting restaurant:', error)
    });
  }

  getRestaurants(): void {
    this.apiService.getRestaurants().subscribe({
      next: (restaurants: Restaurant[]) => {
        this.restaurants = restaurants.filter((restaurant: Restaurant) => restaurant.ownerId === this.ownerId);
      },
      error: (error) => console.error('Error fetching restaurants:', error)
    });
  }

  navigateToRestaurant(restaurantId: number): void {
    this.router.navigate([`/restaurants/${restaurantId}`]);
  }
}
