import { Component, Signal, effect } from '@angular/core';
import { Title } from '@angular/platform-browser';
import {
  InventoryItem,
  InventoryService,
} from '../../services/inventory.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-manage-orders',
  templateUrl: './manage-orders.component.html',
  styleUrl: './manage-orders.component.css',
})
export class ManageOrdersComponent {
  inventoryItems: Signal<InventoryItem[]>;
  inventoryIds: string[] = [];
  newItemName: string = '';

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Manage Orders');
    this.inventoryItems = serviceInventory.InventoryItems;

    effect(() => {
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Manage Orders (${this.inventoryItems().length}) `
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

  onBeginNewOrder() {
    this.router.navigate(['../create'], { relativeTo: this.route });
  }
}
