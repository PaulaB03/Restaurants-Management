import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-restaurant',
  standalone: true,
  imports: [],
  templateUrl: './restaurant.component.html',
  styleUrl: './restaurant.component.css'
})
export class RestaurantComponent {
  restaurant: any;

  constructor(private route: ActivatedRoute, private apiService: ApiService) {}

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id')!; 
    this.getRestaurantById(id);
  }

  getRestaurantById(restaurantId: number): void {
    this.apiService.getRestaurantById(restaurantId).subscribe({
      next: (res) => console.log('Restaurant details:', res),
      error: (error) => console.error('Error fetching restaurant details:', error)
    });
  }
}
