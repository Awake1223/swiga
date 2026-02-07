// Services/user.service.ts - ИСПРАВЛЕННЫЙ ВАРИАНТ
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterRequest, RegisterAdminRequest, RegistrationResponse } from '../Models/registration.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
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

  // Регистрация клиента - OK
  registerClient(request: RegisterRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(
      `${this.apiUrl}/Registration/client`,
      request
    );
  }

  // Регистрация администратора - OK
  registerAdmin(request: RegisterAdminRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(
      `${this.apiUrl}/Registration/admin`,
      request
    );
  }

  // ПРОБЛЕМА: у вас в бэкенде нет такого эндпоинта!
  // getUserProfile(userId: string): Observable<any> {
  //   return this.http.get<any>(
  //     `${this.apiUrl}/Registration/profile/${userId}`,  // НЕТ ТАКОГО ЭНДПОИНТА!
  //     { headers: this.getAuthHeaders() }
  //   );
  // }

  // ИСПРАВЛЕНИЕ: Используйте /api/me (из MeController)
  getProfile(): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/api/me`,  // ТОЧНО ТАКОЙ URL ВАШЕГО MeController!
      { headers: this.getAuthHeaders() }
    );
  }

  // Обновление профиля (если нужно)
  updateProfile(request: any): Observable<any> {
    return this.http.put<any>(
      `${this.apiUrl}/api/me/profile`,
      request,
      { headers: this.getAuthHeaders() }
    );
  }

  // Обновление пользователя (старый метод, если нужен)
  updateUser(id: string, request: any): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/Registration/${id}`,  // Проверьте есть ли такой эндпоинт
      request,
      { headers: this.getAuthHeaders() }
    );
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/Registration/${id}`,
      { headers: this.getAuthHeaders() }
    );
  }
}
