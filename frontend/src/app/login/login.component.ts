import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginData = { email: '', password: '' };

  constructor(private authService: AuthService, private router: Router) { }

  login() {
    this.authService.login(this.loginData).subscribe(response => {
      // Store the token using AuthService
      this.authService.setToken(response.token);
      
      // Check if the user is logged in
      if (this.authService.isLoggedIn()) {
        // Redirect to the home page
        this.router.navigate(['/profile']);
      } else {
        // Handle the case where the token is not set (e.g., display an error message)
        console.error('Token not set');
      }
    }, error => {
      // Handle login error
      console.error('Login error:', error);
    });
  }
}
