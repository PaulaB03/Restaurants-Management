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

  // Product Functions
  addProduct(product: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.baseUrl}/Product`, product, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getProductById(productId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Product/products/${productId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  updatePrice(productId: number, price: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.put(`${this.baseUrl}/Product/${productId}/${price}`, {}, { headers, responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  deleteProduct(productId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/Product/${productId}`, { headers, responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  getCategoryById(categoryId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Product/categories/${categoryId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getCategories(): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Product/GetCategories`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getProductsByRestaurant(restaurantId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Product/restaurant/${restaurantId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getProductsByRestaurantAndCategory(restaurantId: number, categoryId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Product/restaurant/${restaurantId}/category/${categoryId}`, { headers }).pipe(
      catchError(this.handleError)
    );
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

  // Order Functions
  getOrders(userId: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Order/GetOrders/${userId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  getOrderById(orderId: number, userId: string): Observable<any> {
    const headers = this.getHeaders();
    return this.http.get(`${this.baseUrl}/Order/GetOrderById/${orderId}/${userId}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  addOrder(order: any): Observable<any> {
    const headers = this.getHeaders();
    return this.http.post(`${this.baseUrl}/Order/AddOrder`, order, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  deleteOrder(orderId: number): Observable<any> {
    const headers = this.getHeaders();
    return this.http.delete(`${this.baseUrl}/Order/DeleteOrder/${orderId}`, { headers, responseType: 'text' }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(errorMessage);
  }
}
