import { Component, Signal, effect } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { InventoryItem } from '../../services/inventory.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Order, OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-manage-orders',
  templateUrl: './manage-orders.component.html',
  styleUrl: './manage-orders.component.css',
})
export class ManageOrdersComponent {
  orderItems: Signal<Order[]>;
  inventoryIds: string[] = [];
  newItemName: string = '';

  constructor(
    private serviceTitle: Title,
    public serviceOrders: OrderService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.serviceTitle.setTitle('BurgerLink.Ui > Manage Orders');
    this.orderItems = serviceOrders.getAllOrders;

    effect(() => {
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Manage Orders (${this.orderItems().length}) `
      );
    });
  }

  onRowEditInit(inventoryItem: InventoryItem) {}

  onRowEditSave(inventoryItem: InventoryItem) {}

  onRowEditCancel(inventoryItem: InventoryItem, index: number) {}

  onSubmit() {
    // this.serviceInventory.AddInventoryItem(this.newItemName).subscribe();
    this.newItemName = '';
  }

  onBeginNewOrder() {
    this.router.navigate(['../create'], { relativeTo: this.route });
  }
}
