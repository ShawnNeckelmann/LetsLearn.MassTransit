import { Component, Signal, effect } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { InventoryItem, InventoryService } from '../services/inventory.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  inventoryItems: Signal<InventoryItem[]>;
  inventoryIds: string[] = [];
  newItemName: string = '';

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService
  ) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Inventory');
    this.inventoryItems = serviceInventory.InventoryItems;

    effect(() => {
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Inventory (${this.inventoryItems().length}) `
      );
    });
  }

  onRowEditInit(inventoryItem: InventoryItem) {}

  onRowEditSave(inventoryItem: InventoryItem) {
    this.serviceInventory
      .ModifyInventoryItem(
        inventoryItem.id,
        inventoryItem.itemName,
        inventoryItem.quantity
      )
      .subscribe();
  }

  onRowEditCancel(inventoryItem: InventoryItem, index: number) {}

  onSubmit() {
    this.serviceInventory.AddInventoryItem(this.newItemName).subscribe();
    this.newItemName = '';
  }
}
