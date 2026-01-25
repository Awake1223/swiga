import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',  // Внешний HTML файл
  styleUrls: ['./login.component.css']     // Внешний CSS файл]
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

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/profile';
        this.router.navigateByUrl(returnUrl);
      },
      error: (error) => {
        this.error = error.error?.message || 'Ошибка входа. Проверьте email и пароль';
        this.isLoading = false;
      }
    });
  }
}
