import { Routes } from '@angular/router';
import { RegistrationComponent } from './Components/registration/registration.component';
import { RentalPointComponent } from './Components/rental-point/rental-point.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { RentalPointsListComponent } from './Components/rental-points-list/rental-points-list.component';
import { InventoryListComponent } from './Components/inventory-list/inventory-list.component';
import { LoginComponent } from './Components/login/login.component';
import { AuthGuard } from './Guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegistrationComponent },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  {
    path: 'rental-points',
    children: [
      { path: '', component: RentalPointsListComponent },
      { path: 'manage', component: RentalPointComponent, canActivate: [AuthGuard] },
      { path: ':id/inventory', component: InventoryListComponent }
    ]
  },
  { path: '', redirectTo: '/rental-points', pathMatch: 'full' },
  { path: '**', redirectTo: '/rental-points' }
];
