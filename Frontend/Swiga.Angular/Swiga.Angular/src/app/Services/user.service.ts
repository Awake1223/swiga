import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterRequest, RegisterAdminRequest, RegistrationResponse } from '../Models/registration.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7087/Registration';

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

  registerClient(request: RegisterRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(
      `${this.apiUrl}/client`,
      request
    );
  }

  registerAdmin(request: RegisterAdminRequest): Observable<RegistrationResponse> {
    return this.http.post<RegistrationResponse>(
      `${this.apiUrl}/admin`,
      request
    );
  }

  getUserProfile(userId: string): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/profile/${userId}`,
      { headers: this.getAuthHeaders() }
    );
  }

  updateUser(id: string, request: any): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      request,
      { headers: this.getAuthHeaders() }
    );
  }

  deleteUser(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
      { headers: this.getAuthHeaders() }
    );
  }
}
