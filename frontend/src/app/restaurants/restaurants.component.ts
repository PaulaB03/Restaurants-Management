import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

interface Address {
  street: string;
  floor?: string;
  number: string;
  city: string;
}

interface Restaurant {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  address?: Address;
}

@Component({
  selector: 'app-restaurants',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './restaurants.component.html',
  styleUrl: './restaurants.component.css'
})
export class RestaurantsComponent {
  restaurants: Restaurant[] = [];

  constructor(private apiService: ApiService, private router: Router) {}

  ngOnInit(): void {
    this.getRestaurants();
  }

  getRestaurants(): void {
    this.apiService.getRestaurants().subscribe({
      next: (res) =>  {
        console.log('List of restaurants:', res)
        this.restaurants = res;
      },
      error: (error) => console.error('Error fetching restaurants:', error)
    });
  }

  navigateToRestaurant(restaurantId: number): void {
    this.router.navigate([`/restaurants/${restaurantId}`]);
  }

  formatAddress(address: Address): string {
    return `${address.street} ${address.number}${address.floor ? ', Floor: ' + address.floor : ''}, ${address.city}`;
  }
}
