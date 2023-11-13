import { Component } from '@angular/core';
import { MessageService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  products: Product[];
  statuses: SelectItem[];
  clonedProducts: { [s: string]: Product } = {};

  constructor(private messageService: MessageService) {
    this.statuses = [
      { label: 'In Stock', value: 'INSTOCK' },
      { label: 'Low Stock', value: 'LOWSTOCK' },
      { label: 'Out of Stock', value: 'OUTOFSTOCK' },
    ];

    this.products = [];
    this.products.push({
      id: 1,
      code: 'test',
      name: 'first',
      price: 1,
      inventoryStatus: 'INSTOCK',
    });
  }

  onRowEditInit(product: Product) {
    this.clonedProducts[product.id] = { ...product };
  }

  onRowEditSave(product: Product) {
    if (product.price > 0) {
      delete this.clonedProducts[product.id];
      this.messageService.add({
        severity: 'success',
        summary: 'Success',
        detail: 'Product is updated',
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Invalid Price',
      });
    }
  }

  onRowEditCancel(product: Product, index: number) {
    this.products[index] = this.clonedProducts[product.id];
    delete this.clonedProducts[product.id];
  }
}

interface Product {
  id: number;
  code: string;
  name: string;
  price: number;
  inventoryStatus: string;
}
