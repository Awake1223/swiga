// Services/inventory.service.ts - ПРОВЕРКА URL
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, tap } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  // ВАЖНО: Проверьте правильный URL!
  // Ваш InventoryController имеет [Route("[controller]")], значит URL: /Inventory
  // Но также есть ReservationController с [Route("api/[controller]")], значит URL: /api/Reservation
  private apiUrl = 'https://localhost:7087';

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

  // Получить весь инвентарь
  getInventory(): Observable<any> {
    const url = `${this.apiUrl}/Inventory`;
    console.log('Fetching inventory from:', url);

    return this.http.get<any>(url, { headers: this.getAuthHeaders() }).pipe(
      tap({
        next: (response) => {
          console.log('Inventory response:', response);
          console.log('Response type:', typeof response);
          console.log('Response length:', Array.isArray(response) ? response.length : 'Not an array');
        },
        error: (error) => {
          console.error('Inventory error:', error);
          console.log('Error status:', error.status);
          console.log('Error message:', error.message);
        }
      }),
      catchError(error => {
        console.error('Inventory catchError:', error);
        throw error;
      })
    );
  }

  // Получить инвентарь по ID пункта проката 
  // ВАЖНО: У вас нет такого эндпоинта в бэкенде! Нужно фильтровать на фронте или добавить эндпоинт
  getInventoryByRentalPoint(rentalPointId: string): Observable<any> {
    console.log('Fetching inventory for rental point:', rentalPointId);

    // Пока просто получаем весь инвентарь и фильтруем на клиенте
    return this.getInventory();
  }

  // Получить конкретный инвентарь (GET /Inventory/{id})
  getInventoryItem(itemId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/Inventory/${itemId}`);
  }

  // Создать инвентарь (POST /Inventory) - требует авторизации админа
  createInventory(item: any): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/Inventory`,
      item,
      { headers: this.getAuthHeaders() }
    );
  }

  // Обновить инвентарь (PUT /Inventory/{id}) - требует авторизации админа
  updateInventory(itemId: string, item: any): Observable<any> {
    return this.http.put<any>(
      `${this.apiUrl}/Inventory/${itemId}`,
      item,
      { headers: this.getAuthHeaders() }
    );
  }

  // Удалить инвентарь (DELETE /Inventory/{id}) - требует авторизации админа
  deleteInventory(itemId: string): Observable<any> {
    return this.http.delete<any>(
      `${this.apiUrl}/Inventory/${itemId}`,
      { headers: this.getAuthHeaders() }
    );
  }

  // Бронирование инвентаря - ЭТО НЕ ТУТ!
  // Бронирование делается через ReservationController
  // bookItem(itemId: string, bookingData: any): Observable<any> {
  //   return this.http.post<any>(
  //     `${this.apiUrl}/Inventory/${itemId}/book`, 
  //     bookingData, 
  //     { headers: this.getAuthHeaders() }
  //   );
  // }
}
