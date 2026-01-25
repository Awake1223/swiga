import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { InventoryService } from '../../Services/inventory.service';
import { RentalPointService } from '../../Services/rental-point.service';
import { AuthService } from '../../Services/auth.service'; // Добавлен импорт

@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-list.component.html',  // Внешний HTML файл
  styleUrls: ['./inventory-list.component.css']     // Внешний CSS файл
})
export class InventoryListComponent implements OnInit {
  rentalPointId = '';
  rentalPoint: any = null;
  inventoryItems: any[] = [];
  isLoading = true;
  error: string | null = null;
  isAdmin = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private inventoryService: InventoryService,
    private rentalPointService: RentalPointService,
    private authService: AuthService // Теперь AuthService импортирован
  ) { }

  ngOnInit() {
    this.isAdmin = this.authService.getRole() === 'Admin';
    this.rentalPointId = this.route.snapshot.paramMap.get('id') || '';

    if (this.rentalPointId) {
      this.loadRentalPoint();
      this.loadInventory();
    } else {
      this.error = 'Не указан ID точки проката';
      this.isLoading = false;
    }
  }

  loadRentalPoint() {
    this.rentalPointService.getRentalPointById(this.rentalPointId).subscribe({
      next: (point) => {
        this.rentalPoint = point;
      },
      error: (error) => {
        console.error('Ошибка загрузки точки проката', error);
      }
    });
  }

  loadInventory() {
    this.isLoading = true;
    this.inventoryService.getInventoryByRentalPoint(this.rentalPointId).subscribe({
      next: (items) => {
        this.inventoryItems = items;
        this.isLoading = false;
      },
      error: (error) => {
        this.error = 'Ошибка загрузки инвентаря: ' + (error.error?.message || error.message);
        this.isLoading = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/rental-points']);
  }

  addNewItem() {
    this.router.navigate(['/inventory', 'new'], { queryParams: { rentalPointId: this.rentalPointId } });
  }

  editItem(item: any) {
    this.router.navigate(['/inventory', item.id, 'edit']);
  }

  deleteItem(itemId: string) {
    if (confirm('Вы уверены, что хотите удалить этот предмет?')) {
      this.inventoryService.deleteInventory(itemId).subscribe({
        next: () => {
          this.inventoryItems = this.inventoryItems.filter(item => item.id !== itemId);
        },
        error: (error) => {
          alert('Ошибка удаления: ' + (error.error?.message || error.message));
        }
      });
    }
  }

  bookItem(item: any) {
    if (item.amount <= 0) {
      alert('Нет доступного инвентаря для бронирования');
      return;
    }

    // Здесь будет логика бронирования
    alert(`Вы выбрали для бронирования: ${item.name} (${item.size})\nЦена: ${item.pricePerHour} ₽/час`);

    // Пример данных для бронирования
    const bookingData = {
      itemId: item.id,
      userId: this.authService.getCurrentUser()?.userId,
      startDate: new Date().toISOString(),
      endDate: new Date(Date.now() + 2 * 60 * 60 * 1000).toISOString(), // +2 часа
      totalAmount: item.pricePerHour * 2
    };

    console.log('Данные для бронирования:', bookingData);

    // В реальном приложении здесь будет вызов:
    // this.inventoryService.bookItem(item.id, bookingData).subscribe(...)
  }

  getCategoryName(itemName: string): string {
    const lowerName = itemName.toLowerCase();
    if (lowerName.includes('скейт')) return 'Скейтборды';
    if (lowerName.includes('самокат')) return 'Самокаты';
    if (lowerName.includes('велосипед')) return 'Велосипеды';
    if (lowerName.includes('ролики')) return 'Ролики';
    return 'Другое';
  }

  getUnit(itemName: string): string {
    const lowerName = itemName.toLowerCase();
    if (lowerName.includes('скейт') || lowerName.includes('самокат') || lowerName.includes('велосипед') || lowerName.includes('ролики')) {
      return 'шт.';
    }
    return 'шт.';
  }
}
