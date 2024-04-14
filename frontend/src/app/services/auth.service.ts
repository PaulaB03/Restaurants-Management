import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authUrl = 'https://localhost:7004/api/auth'; 
  private tokenPayload: any;

  constructor(private http: HttpClient, private cookieService: CookieService, private router: Router) {}

  register(userData: any): Observable<any> {
    return this.http.post<string>(`${this.authUrl}/register`, userData, { responseType: 'text' as 'json' });
  }  

  login(email: string, password: string): Observable<string> {
    return this.http.post<string>(`${this.authUrl}/login`, { email, password }, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      }),
      responseType: 'text' as 'json'
    });
  }

  saveToken(token: string): void {
    this.cookieService.set('token', token, 1, '/', undefined, false, 'Lax');
  }

  getToken(): string | null {
    return this.cookieService.get('token');
  }

  decodeToken(): void {
    const token = this.getToken();
    if (token) {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));

      this.tokenPayload = JSON.parse(jsonPayload);
    }
  }

  hasRole(role: string): boolean {
    if (!this.tokenPayload) {
      this.decodeToken(); 
    }
    const roles = this.tokenPayload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    return roles ? roles.includes(role) : false;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    this.cookieService.delete('token', '/');
    this.tokenPayload = null;
    this.router.navigate(['login']);
  }

  checkEmailExists(email: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.authUrl}/CheckEmail?email=${encodeURIComponent(email)}`);
  }

  checkUsernameExists(username: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.authUrl}/CheckUsername?username=${encodeURIComponent(username)}`);
  }

  getUserByEmail(): Observable<any> {
    if (!this.tokenPayload) {
      this.decodeToken();  // Ensure token is decoded
    }
    const email = this.tokenPayload?.['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];  // Ensure this claim matches your token structure
    if (!email) {
      throw new Error("Email not found in token");
    }
    return this.http.get<any>(`${this.authUrl}/GetUser?email=${encodeURIComponent(email)}`);
  }
}
