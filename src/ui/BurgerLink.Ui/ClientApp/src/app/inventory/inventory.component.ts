import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  inventoryItems: InventoryItem[];

  constructor(private serviceTitle: Title) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Inventory');
    this.inventoryItems = [];
    this.inventoryItems.push({
      id: 1,
      name: 'first',
      quantity: 10,
    });
  }

  onRowEditInit(inventoryItem: InventoryItem) {
    console.log(inventoryItem);
  }

  onRowEditSave(inventoryItem: InventoryItem) {
    console.log(inventoryItem);
  }

  onRowEditCancel(inventoryItem: InventoryItem, index: number) {
    console.log(inventoryItem);
  }
}

interface InventoryItem {
  id: number;
  name: string;
  quantity: number;
}
