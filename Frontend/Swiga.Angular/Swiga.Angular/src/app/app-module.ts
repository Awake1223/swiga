import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

// Components
import { AppComponent } from './app.component';
import { RegistrationComponent } from './Components/registration/registration.component';
import { RentalPointComponent } from './Components/rental-point/rental-point.component';

// Services
import { UserService } from './Services/user.service';
import { RentalPointService } from './Services/rental-point.service';
import { AuthService } from './Services/auth.service';
import { InventoryService } from './Services/inventory.service';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    // УДАЛЕН AppRoutingModule - он больше не нужен

    // Standalone компоненты
    AppComponent,
    RegistrationComponent,
    RentalPointComponent
  ],
  providers: [
    UserService,
    RentalPointService,
    AuthService,
    InventoryService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
