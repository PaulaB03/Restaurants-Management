import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:7113/api/User'; 
  
  constructor(private http: HttpClient, private cookieService: CookieService, private router: Router) { }

  register(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, user);
  }

  login(loginData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, loginData);
  }

  logout(): void {
    // Remove the token from cookies or local storage
    this.cookieService.delete('token');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    // Check if the user is logged in (token exists)
    return this.cookieService.check('token');

  }

  setToken(token: string): void {
    // Set the token in cookies
    this.cookieService.set('token', token);
  }

  getToken(): string | undefined {
    // Get the token from cookies
    return this.cookieService.get('token');
  }
}