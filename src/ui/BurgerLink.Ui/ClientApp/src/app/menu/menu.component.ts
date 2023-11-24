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
  menuItemOptions: Signal<InventoryItem[]>;
  selectedMenuItems: InventoryItem[];

  constructor(
    private serviceTitle: Title,
    public serviceInventory: InventoryService
  ) {
    this.selectedMenuItems = [];
    this.menuItemOptions = serviceInventory.InventoryItems;

    this.serviceTitle.setTitle(
      `BurgerLink.Ui > Inventory (${this.menuItemOptions().length}) `
    );

    effect(() => {
      this.serviceTitle.setTitle(
        `BurgerLink.Ui > Menu (${this.menuItemOptions().length}) `
      );
    });
  }
}
