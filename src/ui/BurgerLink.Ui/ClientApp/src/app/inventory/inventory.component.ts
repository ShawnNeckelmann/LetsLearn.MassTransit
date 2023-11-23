import { Component, Signal, effect } from '@angular/core';
import { Title } from '@angular/platform-browser';
import {
  InventoryItem,
  InventoryService,
} from '../services/InventoryService.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  inventoryItems: Signal<InventoryItem[]>;
  inventoryIds: string[] = [];
  newItemName: string = crypto.randomUUID();

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService,
    private messageService: MessageService
  ) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Inventory');
    this.inventoryItems = serviceInventory.InventoryItems;

    effect(() => {
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Inventory (${this.inventoryItems().length}) `
      );

      if (this.inventoryIds.length === 0) {
        this.inventoryIds = this.inventoryItems().map((item) => item.id);
        return;
      }

      const difference = this.inventoryItems()
        .map((i) => i.id)
        .filter((id) => !this.inventoryIds.includes(id));

      difference.forEach((id) => {
        const item = this.inventoryItems().filter((item) => item.id === id)[0];
        this.messageService.add({
          severity: 'success',
          summary: 'New item!',
          detail: `Just added ${item.quantity} ${item.itemName} to the inventory.`,
        });
      });
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

  onSubmit() {
    this.serviceInventory.AddInventoryItem(this.newItemName).subscribe();
    // this.newItemName = '';
    this.newItemName = crypto.randomUUID();
  }
}
