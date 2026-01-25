import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { UserService } from '../../Services/user.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.component.html',  // Внешний HTML файл
  styleUrls: ['./profile.component.css']     // Внешний CSS файл]
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
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.userService.getUserProfile(currentUser.userId).subscribe({
        next: (userData) => {
          this.user = { ...currentUser, ...userData };
          this.isLoading = false;
        },
        error: (error) => {
          this.error = 'Ошибка загрузки профиля: ' + (error.error?.message || error.message);
          this.isLoading = false;
        }
      });
    } else {
      this.isLoading = false;
      this.error = 'Пользователь не авторизован';
    }
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
