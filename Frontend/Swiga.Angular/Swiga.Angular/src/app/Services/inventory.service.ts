import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private apiUrl = 'https://localhost:7087/Inventory';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getInventoryByRentalPoint(rentalPointId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/by-rental-point/${rentalPointId}`);
  }

  getInventoryItem(itemId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${itemId}`);
  }

  createInventory(item: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, item, { headers: this.getAuthHeaders() });
  }

  updateInventory(itemId: string, item: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${itemId}`, item, { headers: this.getAuthHeaders() });
  }

  deleteInventory(itemId: string): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${itemId}`, { headers: this.getAuthHeaders() });
  }

  bookItem(itemId: string, bookingData: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${itemId}/book`, bookingData, { headers: this.getAuthHeaders() });
  }
}
