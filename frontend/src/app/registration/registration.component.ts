import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  user = { username: '', email: '', password: '', firstName: '', lastName: ''};

  constructor(private authService: AuthService, private router: Router) { }

  register(): void {
    this.authService.register(this.user).subscribe(response => {
      // Handle successful registration (you can redirect to login page)
      console.log('Registration successful:', response);

      // Go to login page
      this.router.navigate(['/login']);
    }, error => {
      // Handle registration error
      console.error('Registration error:', error);
    });
  }
}
