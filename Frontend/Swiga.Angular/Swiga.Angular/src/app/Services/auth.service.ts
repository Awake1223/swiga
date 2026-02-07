// services/auth.service.ts - ОБНОВЛЕННЫЙ
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7087';
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    // При запуске проверяем localStorage
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const token = localStorage.getItem('access_token');
    if (token) {
      const user = {
        id: localStorage.getItem('user_id'),
        email: localStorage.getItem('user_email'),
        role: localStorage.getItem('user_role')
      };
      this.currentUserSubject.next(user);
    }
  }

  login(credentials: { email: string, password: string }): Observable<any> {
    console.log('Login attempt:', credentials.email); // Логируем

    return this.http.post(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap({
        next: (response: any) => {
          console.log('Login response:', response); // Логируем ответ

          // Сохраняем данные
          localStorage.setItem('access_token', response.accessToken);
          localStorage.setItem('user_id', response.userId);
          localStorage.setItem('user_email', response.email);
          localStorage.setItem('user_role', response.role);

          // Обновляем состояние
          const user = {
            id: response.userId,
            email: response.email,
            role: response.role,
            token: response.accessToken
          };

          this.currentUserSubject.next(user);
          console.log('User saved to localStorage and state');
        },
        error: (error) => {
          console.error('Login error:', error);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_id');
    localStorage.removeItem('user_email');
    localStorage.removeItem('user_role');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('access_token');
    const hasToken = !!token;
    console.log('isLoggedIn check:', { hasToken, token: token?.substring(0, 20) + '...' });
    return hasToken;
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  getCurrentUser(): any {
    return {
      id: localStorage.getItem('user_id'),
      email: localStorage.getItem('user_email'),
      role: localStorage.getItem('user_role')
    };
  }

  getRole(): string | null {
    return localStorage.getItem('user_role');
  }

  isAdmin(): boolean {
    const role = this.getRole();
    return role === 'Admin';
  }

  getUserId(): string | null {
    return localStorage.getItem('user_id');
  }
}
