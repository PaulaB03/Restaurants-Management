import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  userDetails: any;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.getUserByEmail().subscribe({
      next: user => {
        this.userDetails = user;
        console.log(this.userDetails);
      },
      error: error => {
        console.error('Failed to fetch user details', error);
      }
    });
  }
}
