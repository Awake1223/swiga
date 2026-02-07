// Components/profile/profile.component.ts - ИСПРАВЛЕННЫЙ
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { UserService } from '../../Services/user.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  user: any = null;
  isLoading = true;
  error: string | null = null;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) { }

  ngOnInit() {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadUserProfile();
  }

  loadUserProfile() {
    this.isLoading = true;
    this.error = null;

    // Получаем базовую информацию из AuthService
    const currentUser = this.authService.getCurrentUser();
    console.log('Basic user info from AuthService:', currentUser);

    // Получаем полный профиль с сервера
    this.userService.getProfile().subscribe({
      next: (profileData) => {
        console.log('Full profile from API:', profileData);
        // Объединяем базовую информацию и данные профиля
        this.user = { ...currentUser, ...profileData };
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.error = 'Ошибка загрузки профиля: ' + (error.error?.message || error.message || 'Неизвестная ошибка');
        this.isLoading = false;

        // Если ошибка 401 (не авторизован), перенаправляем на логин
        if (error.status === 401) {
          this.authService.logout();
        }
      }
    });
  }

  logout() {
    this.authService.logout();
  }

  registerRentalPoint() {
    this.router.navigate(['/rental-points/manage']);
  }

  viewRentalPoints() {
    this.router.navigate(['/rental-points']);
  }
}
