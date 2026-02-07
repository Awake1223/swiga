// components/login/login.component.ts - ОБНОВЛЕННЫЙ
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  login() {
    if (!this.email || !this.password) {
      this.error = 'Пожалуйста, заполните все поля';
      return;
    }

    this.isLoading = true;
    this.error = '';

    console.log('Attempting login with:', { email: this.email });

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (response) => {
        console.log('Login successful, response:', response);
        this.isLoading = false;

        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/profile';
        console.log('Navigating to:', returnUrl);
        this.router.navigateByUrl(returnUrl);
      },
      error: (error) => {
        console.error('Login failed:', error);
        this.isLoading = false;

        // Подробный вывод ошибки
        if (error.status === 401) {
          this.error = 'Неверный email или пароль';
        } else if (error.status === 400) {
          this.error = error.error?.error || 'Ошибка в запросе';
        } else if (error.status === 0) {
          this.error = 'Не удалось подключиться к серверу. Проверьте, запущен ли бэкенд.';
        } else {
          this.error = 'Ошибка входа: ' + (error.error?.error || error.message || 'Неизвестная ошибка');
        }
      },
      complete: () => {
        console.log('Login observable completed');
      }
    });
  }
}
