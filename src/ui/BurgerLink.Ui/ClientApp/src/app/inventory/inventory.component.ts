import { Component, WritableSignal, effect } from '@angular/core';
import { Title } from '@angular/platform-browser';
import {
  InventoryItem,
  InventoryService,
} from '../services/InventoryService.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  inventoryItems: InventoryItem[] = [];
  newItemName: string = '';

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService
  ) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Inventory');
    this.inventoryItems = serviceInventory.InventoryItems();
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

  onSubmit() {
    this.serviceInventory.AddInventoryItem(this.newItemName).subscribe();
    this.newItemName = '';
  }
}
