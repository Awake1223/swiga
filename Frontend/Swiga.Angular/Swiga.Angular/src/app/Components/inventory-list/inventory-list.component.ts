// Components/inventory-list/inventory-list.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../Services/auth.service';
import { InventoryService } from '../../Services/inventory.service';

@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})
export class InventoryListComponent implements OnInit {
  rentalPointId: string = '';
  inventory: any[] = [];
  isLoading = true;
  error: string | null = null;
  isAdmin = false;
  userRole: string | null = null;

  constructor(
    private authService: AuthService,
    private inventoryService: InventoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    console.log('InventoryListComponent created');
  }

  ngOnInit(): void {
    console.log('ngOnInit started');

    // Получаем роль пользователя
    this.userRole = this.authService.getRole();
    this.isAdmin = this.authService.isAdmin();

    console.log('User role:', this.userRole);
    console.log('Is admin:', this.isAdmin);

    // Получаем rentalPointId из параметров маршрута
    this.route.params.subscribe(params => {
      this.rentalPointId = params['id'];
      console.log('Rental point ID from route:', this.rentalPointId);
      this.loadInventory();
    });
  }

  loadInventory(): void {
    console.log('Loading inventory...');

    this.isLoading = true;
    this.error = null;
    this.inventory = [];

    this.inventoryService.getInventory().subscribe({
      next: (data) => {
        console.log('Inventory data received:', data);

        // Проверяем, что data существует и является массивом
        if (data && Array.isArray(data)) {
          // Фильтруем инвентарь по rentalPointId (если он указан)
          if (this.rentalPointId) {
            this.inventory = data.filter((item: any) =>
              item.rentalPointId === this.rentalPointId
            );
            console.log(`Filtered inventory for rental point ${this.rentalPointId}:`, this.inventory.length, 'items');
          } else {
            this.inventory = data;
            console.log('All inventory loaded:', this.inventory.length, 'items');
          }
        } else if (data && typeof data === 'object') {
          // Если ответ - объект, возможно, это { items: [...] }
          if (data.items && Array.isArray(data.items)) {
            this.inventory = data.items;
            console.log('Inventory from items property:', this.inventory.length, 'items');
          } else {
            // Пробуем преобразовать в массив
            this.inventory = Object.values(data);
            console.log('Converted object to array:', this.inventory.length, 'items');
          }
        } else {
          console.warn('Unexpected data format:', data);
          this.inventory = [];
        }

        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading inventory:', error);

        if (error.status === 401) {
          this.error = 'Необходима авторизация. Пожалуйста, войдите в систему.';
        } else if (error.status === 403) {
          this.error = 'У вас нет доступа к этому ресурсу.';
        } else if (error.status === 404) {
          this.error = 'Инвентарь не найден.';
        } else if (error.status === 0) {
          this.error = 'Не удалось подключиться к серверу. Проверьте соединение.';
        } else {
          this.error = 'Ошибка загрузки инвентаря: ' +
            (error.error?.error || error.error?.message || error.message || 'Неизвестная ошибка');
        }

        this.isLoading = false;
      },
      complete: () => {
        console.log('Inventory loading completed');
      }
    });
  }

  // Метод для создания нового инвентаря (только для админов)
  createInventory(): void {
    console.log('Create inventory clicked');
    this.router.navigate(['/inventory/create'], {
      queryParams: { rentalPointId: this.rentalPointId }
    });
  }

  // Метод для редактирования инвентаря (только для админов)
  editInventory(itemId: string): void {
    console.log('Edit inventory clicked for item:', itemId);
    this.router.navigate(['/inventory/edit', itemId]);
  }

  // Метод для удаления инвентаря (только для админов)
  deleteInventory(itemId: string): void {
    console.log('Delete inventory clicked for item:', itemId);

    if (confirm('Вы уверены, что хотите удалить этот инвентарь?')) {
      this.inventoryService.deleteInventory(itemId).subscribe({
        next: () => {
          console.log('Item deleted successfully');
          this.loadInventory(); // Перезагружаем список
        },
        error: (error) => {
          console.error('Error deleting item:', error);
          alert('Ошибка удаления: ' + (error.error?.message || error.message || 'Неизвестная ошибка'));
        }
      });
    }
  }

  // Метод для бронирования (только для клиентов)
  bookItem(item: any): void {
    console.log('Book item clicked:', item);
    this.router.navigate(['/reservation/create'], {
      queryParams: {
        inventoryId: item.id,
        rentalPointId: item.rentalPointId
      }
    });
  }

  // Метод для обновления списка
  refreshInventory(): void {
    console.log('Refreshing inventory...');
    this.loadInventory();
  }
}
