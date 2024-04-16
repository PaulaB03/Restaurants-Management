import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7004/api';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken(); 
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  // Admin Functions
  assignRole(email: string, newRole: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.baseUrl}/Admin/AssignRole/${encodeURIComponent(email)}/${newRole}`, {}, { headers, responseType: 'text' });
  }

  removeRole(email: string, role: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/Admin/RemoveRole/${encodeURIComponent(email)}/${role}`, { headers, responseType: 'text' });
  }

  // Restaurant Functions
  addRestaurant(restaurant: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.baseUrl}/Restaurant`, restaurant, { headers });
  }
  
  deleteRestaurant(restaurantId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/Restaurant/${restaurantId}`, { headers, responseType: 'text' });
  }
  
  getRestaurantById(restaurantId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Restaurant/${restaurantId}`, { headers });
  }
  
  getRestaurants(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Restaurant`, { headers });
  }
}
