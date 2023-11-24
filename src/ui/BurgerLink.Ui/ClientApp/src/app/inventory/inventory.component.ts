import { Component, Signal, effect } from '@angular/core';
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
  inventoryItems: Signal<InventoryItem[]>;
  inventoryIds: string[] = [];
  newItemName: string = crypto.randomUUID();

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

  onRowEditInit(inventoryItem: InventoryItem) {
    console.log(`onRowEditInit: ${JSON.stringify(inventoryItem)}`);
  }

  onRowEditSave(inventoryItem: InventoryItem) {
    console.log(`onRowEditSave: ${JSON.stringify(inventoryItem)}`);
    this.serviceInventory
      .ModifyInventoryItem(
        inventoryItem.id,
        inventoryItem.itemName,
        inventoryItem.quantity
      )
      .subscribe();
  }

  onRowEditCancel(inventoryItem: InventoryItem, index: number) {
    console.log(`onRowEditCancel: ${JSON.stringify(inventoryItem)}`);
  }

  onSubmit() {
    this.serviceInventory.AddInventoryItem(this.newItemName).subscribe();
    this.newItemName = '';
  }
}
