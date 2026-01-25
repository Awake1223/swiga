export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber?: string;
}

export interface RegisterAdminRequest extends RegisterRequest {
  rentalPointId?: string | null;
  createNewRentalPoint?: boolean;
  rentalPointName?: string;
  rentalPointAddress?: string;
  rentalPointCity?: string;
}

export interface RegistrationResponse {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  message: string;
  registeredAt: string;
  rentalPointId?: string;
}
