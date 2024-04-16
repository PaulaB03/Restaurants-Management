import { Component } from '@angular/core';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-owner',
  standalone: true,
  imports: [],
  templateUrl: './owner.component.html',
  styleUrl: './owner.component.css'
})
export class OwnerComponent {
  ownerId: any;

  constructor(private authService: AuthService, private apiService: ApiService) {}

  ngOnInit(): void {
    this.authService.getUserByEmail().subscribe({
      next: user => {
        this.ownerId = user.id;
      },
      error: error => {
        console.error('Failed to fetch user details', error);
      }
    });
  }


  addRestaurant(): void {
    const newRestaurant = {
      name: "Sample Restaurant",
      description: "A fine place to enjoy exquisite meals.",
      ownerId: this.ownerId,
      address: {
        street: "1234 Culinary Blvd",
        floor: "1st",
        number: "100A",
        city: "Gastronomy City"
      }
    };
    this.apiService.addRestaurant(newRestaurant).subscribe({
      next: (res) => console.log('Restaurant added:', res),
      error: (error) => console.error('Error adding restaurant:', error)
    });
  }

  deleteRestaurant(): void {
    const restaurantId = 4;  // trebuie pus id-ul corect
    this.apiService.deleteRestaurant(restaurantId).subscribe({
      next: (res) => console.log(res),
      error: (error) => console.error('Error deleting restaurant:', error)
    });
  }

  // Functia asta o sa o mutam
  getRestaurantById(): void {
    const restaurantId = 1;  
    this.apiService.getRestaurantById(restaurantId).subscribe({
      next: (res) => console.log('Restaurant details:', res),
      error: (error) => console.error('Error fetching restaurant details:', error)
    });
  }

  // O sa fie mutata in restaurants
  getRestaurants(): void {
    this.apiService.getRestaurants().subscribe({
      next: (res) =>  {
        console.log('List of restaurants:', res)
        // O sa salvam datele si le ducem in frontend
      },
      error: (error) => console.error('Error fetching restaurants:', error)
    });
  }
}
