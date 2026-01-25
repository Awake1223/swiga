import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RentalPointService } from '../../Services/rental-point.service';
import { RentalPointRequest, RentalPointResponse } from '../../Models/rental-point.model';
import { AuthService } from '../../Services/auth.service';

@Component({
  selector: 'app-rental-point',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './rental-point.component.html',
  styleUrls: ['./rental-point.component.css']
})
export class RentalPointComponent implements OnInit {
  rentalPoints: RentalPointResponse[] = [];
  showForm = false;
  isEditing = false;
  editingId?: string;

  rentalPointForm: RentalPointRequest = {
    name: '',
    address: '',
    city: '',
    phoneNumber: '',
    email: ''
  };

  errorMessage = '';
  successMessage = '';

  constructor(
    private rentalPointService: RentalPointService,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit() {
    if (this.authService.getRole() !== 'Admin') {
      this.router.navigate(['/rental-points']);
      return;
    }
    this.loadRentalPoints();
  }

  loadRentalPoints() {
    this.rentalPointService.getRentalPoints().subscribe({
      next: (points) => {
        this.rentalPoints = points;
      },
      error: (error) => {
        this.showError('Ошибка загрузки точек проката: ' + (error.error?.message || error.message));
      }
    });
  }

  createRentalPoint() {
    this.rentalPointService.createRentalPoint(this.rentalPointForm).subscribe({
      next: (id) => {
        this.showSuccess('Точка проката успешно создана!');
        this.resetForm();
        this.loadRentalPoints();
      },
      error: (error) => {
        this.showError('Ошибка создания точки проката: ' + (error.error?.message || error.message));
      }
    });
  }

  editRentalPoint(point: RentalPointResponse) {
    this.isEditing = true;
    this.editingId = point.id;
    this.rentalPointForm = {
      name: point.name,
      address: point.address,
      city: point.city,
      phoneNumber: point.phoneNumber,
      email: point.email
    };
    this.showForm = true;
  }

  updateRentalPoint() {
    if (!this.editingId) return;

    this.rentalPointService.updateRentalPoint(this.editingId, this.rentalPointForm).subscribe({
      next: (id) => {
        this.showSuccess('Точка проката успешно обновлена!');
        this.resetForm();
        this.loadRentalPoints();
      },
      error: (error) => {
        this.showError('Ошибка обновления точки проката: ' + (error.error?.message || error.message));
      }
    });
  }

  deleteRentalPoint(id: string) {
    if (confirm('Вы уверены, что хотите удалить эту точку проката?')) {
      this.rentalPointService.deleteRentalPoint(id).subscribe({
        next: () => {
          this.showSuccess('Точка проката успешно удалена!');
          this.loadRentalPoints();
        },
        error: (error) => {
          this.showError('Ошибка удаления точки проката: ' + (error.error?.message || error.message));
        }
      });
    }
  }

  submitForm() {
    if (this.isEditing) {
      this.updateRentalPoint();
    } else {
      this.createRentalPoint();
    }
  }

  resetForm() {
    this.rentalPointForm = {
      name: '',
      address: '',
      city: '',
      phoneNumber: '',
      email: ''
    };
    this.showForm = false;
    this.isEditing = false;
    this.editingId = undefined;
    this.errorMessage = '';
    this.successMessage = '';
  }

  showError(message: string) {
    this.errorMessage = message;
    this.successMessage = '';
    setTimeout(() => this.errorMessage = '', 5000);
  }

  showSuccess(message: string) {
    this.successMessage = message;
    this.errorMessage = '';
    setTimeout(() => this.successMessage = '', 5000);
  }

  // ДОБАВЛЕН МЕТОД goBack
  goBack() {
    this.router.navigate(['/rental-points']);
  }
}
