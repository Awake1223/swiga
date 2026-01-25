import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from './Services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar">
      <div class="container">
        <a routerLink="/" class="logo">SWIGA</a>
        
        <div class="nav-links">
          <a routerLink="/rental-points" routerLinkActive="active">🏪 Пункты проката</a>
          
          <ng-container *ngIf="isLoggedIn">
            <a routerLink="/profile" routerLinkActive="active">👤 Профиль</a>
            <button (click)="logout()" class="logout-btn">Выйти</button>
          </ng-container>
          
          <ng-container *ngIf="!isLoggedIn">
            <a routerLink="/login" routerLinkActive="active">🔑 Вход</a>
            <a routerLink="/register" routerLinkActive="active" class="register-btn">📝 Регистрация</a>
          </ng-container>
        </div>
        
        <button class="mobile-menu-btn" (click)="toggleMenu()">
          <span class="menu-icon">☰</span>
        </button>
      </div>
    </nav>

    <div class="content-container">
      <router-outlet></router-outlet>
    </div>

    <footer class="footer">
      <div class="container">
        <p>&copy; {{ currentYear }} Swiga. Все права защищены.</p>
      </div>
    </footer>
  `,
  styles: [`
    .navbar {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      padding: 1rem 0;
      position: sticky;
      top: 0;
      z-index: 1000;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }
    
    .container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 0 20px;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }
    
    .logo {
      color: white;
      font-size: 24px;
      font-weight: 700;
      text-decoration: none;
    }
    
    .nav-links {
      display: flex;
      gap: 2rem;
      align-items: center;
    }
    
    .nav-links a {
      color: white;
      text-decoration: none;
      font-weight: 500;
      padding: 8px 12px;
      border-radius: 8px;
      transition: all 0.3s;
    }
    
    .nav-links a:hover, 
    .nav-links a.active {
      background: rgba(255,255,255,0.2);
    }
    
    .logout-btn, .register-btn {
      background: white;
      color: #667eea;
      border: none;
      padding: 8px 16px;
      border-radius: 8px;
      font-weight: 600;
      cursor: pointer;
      transition: all 0.3s;
    }
    
    .logout-btn:hover {
      background: rgba(255,255,255,0.9);
      transform: translateY(-2px);
    }
    
    .register-btn {
      background: linear-gradient(135deg, #4299e1 0%, #667eea 100%);
      color: white;
    }
    
    .register-btn:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(66, 153, 225, 0.3);
    }
    
    .mobile-menu-btn {
      display: none;
      background: none;
      border: none;
      color: white;
      font-size: 1.5rem;
      cursor: pointer;
    }
    
    .content-container {
      min-height: calc(100vh - 120px);
      padding: 2rem 0;
    }
    
    .footer {
      background: #1a202c;
      color: #a0aec0;
      text-align: center;
      padding: 1.5rem 0;
      margin-top: auto;
    }
    
    @media (max-width: 768px) {
      .nav-links {
        display: none;
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        flex-direction: column;
        padding: 1rem 0;
        gap: 1rem;
      }
      
      .nav-links.show {
        display: flex;
      }
      
      .mobile-menu-btn {
        display: block;
      }
      
      .container {
        flex-wrap: wrap;
      }
    }
  `]
})
export class AppComponent {
  isLoggedIn = false;
  currentYear = new Date().getFullYear();

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    this.authService.currentUser$.subscribe(user => {
      this.isLoggedIn = !!user;
    });
  }

  toggleMenu() {
    const navLinks = document.querySelector('.nav-links');
    if (navLinks) {
      navLinks.classList.toggle('show');
    }
  }

  logout() {
    this.authService.logout();
  }
}
