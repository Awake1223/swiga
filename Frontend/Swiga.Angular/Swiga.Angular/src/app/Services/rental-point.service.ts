import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { RentalPointRequest, RentalPointResponse } from '../Models/rental-point.model';

@Injectable({
  providedIn: 'root'
})
export class RentalPointService {
  private apiUrl = 'https://localhost:7087/RentalPoint';

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

  getRentalPoints(): Observable<RentalPointResponse[]> {
    return this.http.get<RentalPointResponse[]>(this.apiUrl);
  }

  getRentalPointById(id: string): Observable<RentalPointResponse> {
    return this.http.get<RentalPointResponse>(`${this.apiUrl}/${id}`);
  }

  createRentalPoint(request: RentalPointRequest): Observable<string> {
    // УБЕРИТЕ ЗАГОЛОВКИ АВТОРИЗАЦИИ - пользователь еще не зарегистрирован!
    return this.http.post<string>(this.apiUrl, request);
  }

  updateRentalPoint(id: string, request: RentalPointRequest): Observable<string> {
    return this.http.put<string>(
      `${this.apiUrl}/${id}`,
      request,
      { headers: this.getAuthHeaders() }
    );
  }

  deleteRentalPoint(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
      { headers: this.getAuthHeaders() }
    );
  }
}
