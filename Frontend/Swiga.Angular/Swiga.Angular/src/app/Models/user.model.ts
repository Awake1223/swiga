export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  role: 'Client' | 'Admin';
  createdAt: string;
}

export interface Client extends User {
  dateOfBirth?: string;
  passportData?: string;
  driverLicense?: string;
}

export interface Admin extends User {
  rentalPointId: string;
}
