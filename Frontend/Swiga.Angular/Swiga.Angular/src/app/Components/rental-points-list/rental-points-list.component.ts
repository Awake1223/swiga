import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { RentalPointService } from '../../Services/rental-point.service';
import { AuthService } from '../../Services/auth.service'; // Добавлен импорт

@Component({
  selector: 'app-rental-points-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rental-points-list.component.html',  // Внешний HTML файл
  styleUrls: ['./rental-points-list.component.css']     // Внешний CSS файл
})
export class RentalPointsListComponent implements OnInit {
  rentalPoints: any[] = [];
  isLoading = true;
  error: string | null = null;
  isAdmin = false;

  constructor(
    private rentalPointService: RentalPointService,
    private router: Router,
    private authService: AuthService // Теперь AuthService импортирован
  ) { }

  ngOnInit() {
    this.isAdmin = this.authService.getRole() === 'Admin';
    this.loadRentalPoints();
  }

  loadRentalPoints() {
    this.isLoading = true;
    this.rentalPointService.getRentalPoints().subscribe({
      next: (points) => {
        this.rentalPoints = points;
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Ошибка загрузки пунктов проката: ' + (error.error?.message || error.message);
        this.isLoading = false;
      }
    });
  }

  refreshPoints() {
    this.error = null;
    this.loadRentalPoints();
  }

  viewPointInventory(pointId: string) {
    this.router.navigate(['/rental-points', pointId, 'inventory']);
  }

  goToManagePoints() {
    this.router.navigate(['/rental-points/manage']);
  }
}
