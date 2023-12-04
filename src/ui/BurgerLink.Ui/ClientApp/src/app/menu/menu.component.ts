import { Component, Signal, effect } from '@angular/core';
import { InventoryItem, InventoryService } from '../services/inventory.service';
import { Title } from '@angular/platform-browser';
import { CreateOrder, OrderService } from '../services/order.service';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
})
export class MenuComponent {
  orderName: string = '';
  signalMenuItemOptions: Signal<InventoryItem[]>;
  menuItemOptions: InventoryItem[] = [];
  selectedMenuItems: InventoryItem[];

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService,
    private serviceOrder: OrderService,
    private serviceRoute: Router,
    private route: ActivatedRoute
  ) {
    this.selectedMenuItems = [];
    this.signalMenuItemOptions = serviceInventory.InventoryItems;

    this.serviceTitle.setTitle(
      `BurgerLink.Ui > Menu (${this.signalMenuItemOptions().length}) `
    );

    effect(() => {
      this.menuItemOptions = this.signalMenuItemOptions();
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Menu (${this.signalMenuItemOptions().length}) `
      );
    });
  }

  public onCreateOrder() {
    const ids = this.selectedMenuItems.map((item) => item.id);

    this.serviceOrder.createOrder(this.orderName, ids).subscribe((orderId) => {
      // append the orderId to the current route
      this.serviceRoute.navigate([orderId], { relativeTo: this.route });
    });
  }
}
