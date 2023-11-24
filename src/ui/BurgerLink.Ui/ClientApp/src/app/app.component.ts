import { Component, Signal, effect } from '@angular/core';
import { MessageService } from 'primeng/api';
import {
  InventoryItem,
  InventoryService,
} from './services/InventoryService.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  inventoryIds: string[] = [];
  inventoryItems: Signal<InventoryItem[]>;

  constructor(
    serviceInventory: InventoryService,
    private messageService: MessageService
  ) {
    this.inventoryItems = serviceInventory.InventoryItems;

    effect(() => {
      this.notifyInventoryItemAdded();
    });
  }

  private notifyInventoryItemAdded(): void {
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
        severity: 'info',
        summary: 'New item!',
        detail: `Just added ${item.quantity} ${item.itemName} to the inventory.`,
      });
    });

    this.inventoryIds = this.inventoryItems().map((item) => item.id);
  }
}
