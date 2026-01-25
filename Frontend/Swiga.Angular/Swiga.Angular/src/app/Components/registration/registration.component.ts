import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { RentalPointService } from '../../Services/rental-point.service';
import { RegisterRequest, RegisterAdminRequest, RegistrationResponse } from '../../Models/registration.model';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  userType: 'client' | 'admin' = 'client';
  isLoading = false;

  // ИСПОЛЬЗУЕМ ТОЛЬКО ОДНУ МОДЕЛЬ
  formData: RegisterAdminRequest = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    phoneNumber: '',
    createNewRentalPoint: false,
    rentalPointName: '',
    rentalPointAddress: '',
    rentalPointCity: '',
    rentalPointId: null
  };

  response?: RegistrationResponse;
  error?: string;

  constructor(
    private userService: UserService,
    private rentalPointService: RentalPointService,
    private router: Router
  ) { }

  register() {
    this.isLoading = true;
    this.error = undefined;
    this.response = undefined;

    if (this.userType === 'client') {
      const clientData: RegisterRequest = {
        firstName: this.formData.firstName,
        lastName: this.formData.lastName,
        email: this.formData.email,
        password: this.formData.password,
        phoneNumber: this.formData.phoneNumber
      };

      this.userService.registerClient(clientData).subscribe({
        next: (response) => this.handleSuccess(response),
        error: (error) => this.handleError(error)
      });
    } else {
      if (this.formData.createNewRentalPoint) {
        this.createRentalPointAndRegisterAdmin();
      } else {
        this.userService.registerAdmin(this.formData).subscribe({
          next: (response) => this.handleSuccess(response),
          error: (error) => this.handleError(error)
        });
      }
    }
  }

  private createRentalPointAndRegisterAdmin() {
    const rentalPointRequest = {
      name: this.formData.rentalPointName!,
      address: this.formData.rentalPointAddress!,
      city: this.formData.rentalPointCity!,
      phoneNumber: this.formData.phoneNumber || this.formData.email || '1234567890',
      email: this.formData.email || 'rentalpoint@example.com'
    };

    // Валидация на фронтенде
    if (!this.formData.firstName?.trim()) {
      this.handleError({ error: { message: 'Имя обязательно' } });
      return;
    }
    if (!this.formData.lastName?.trim()) {
      this.handleError({ error: { message: 'Фамилия обязательна' } });
      return;
    }
    if (!this.formData.email?.trim()) {
      this.handleError({ error: { message: 'Email обязателен' } });
      return;
    }
    if (!this.formData.password || this.formData.password.length < 6) {
      this.handleError({ error: { message: 'Пароль должен быть не менее 6 символов' } });
      return;
    }
    if (!rentalPointRequest.phoneNumber?.trim()) {
      this.handleError({ error: { message: 'Телефон обязателен для точки проката' } });
      return;
    }
    if (!rentalPointRequest.email?.trim()) {
      this.handleError({ error: { message: 'Email обязателен для точки проката' } });
      return;
    }

    this.rentalPointService.createRentalPoint(rentalPointRequest).subscribe({
      next: (rentalPointId) => {
        const adminRequest: RegisterAdminRequest = {
          ...this.formData,
          rentalPointId: rentalPointId
        };

        this.userService.registerAdmin(adminRequest).subscribe({
          next: (response) => this.handleSuccess(response),
          error: (error) => this.handleError(error)
        });
      },
      error: (error) => {
        this.error = 'Ошибка создания точки проката: ' + (error.error?.message || error.message);
        this.isLoading = false;
      }
    });
  }

  private handleSuccess(response: RegistrationResponse) {
    this.response = response;
    this.isLoading = false;
    localStorage.setItem('currentUser', JSON.stringify(response));
    setTimeout(() => this.router.navigate(['/profile']), 2000);
  }

  private handleError(error: any) {
    this.error = error.error?.message || 'Ошибка регистрации';
    this.isLoading = false;
  }
}
