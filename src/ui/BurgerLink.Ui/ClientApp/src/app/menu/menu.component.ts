import { Component, Signal, effect } from '@angular/core';
import {
  InventoryItem,
  InventoryService,
} from '../services/InventoryService.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
})
export class MenuComponent {
  orderName: string = 'Order Name';
  signalMenuItemOptions: Signal<InventoryItem[]>;
  menuItemOptions: InventoryItem[] = [];
  selectedMenuItems: InventoryItem[];

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService
  ) {
    this.selectedMenuItems = [];
    this.signalMenuItemOptions = serviceInventory.InventoryItems;

    this.serviceTitle.setTitle(
      `BurgerLink.Ui > Inventory (${this.signalMenuItemOptions().length}) `
    );

    effect(() => {
      this.menuItemOptions = this.signalMenuItemOptions();
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Menu (${this.signalMenuItemOptions().length}) `
      );
    });
  }
}
