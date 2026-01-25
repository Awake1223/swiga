export interface RentalPoint {
  id?: string;
  name: string;
  address: string;
  city: string;
  phoneNumber: string;
  email: string;
}

export interface RentalPointRequest {
  name: string;
  address: string;
  city: string;
  phoneNumber: string;
  email: string;
}

export interface RentalPointResponse {
  id: string;
  name: string;
  address: string;
  city: string;
  phoneNumber: string;
  email: string;
}
