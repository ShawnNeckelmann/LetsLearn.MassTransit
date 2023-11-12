import { Component } from '@angular/core';
import { InventoryService } from './services/InventoryService.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'app';

  constructor(public svcInventory: InventoryService) {}

  ngOnInit() {
    this.svcInventory.onInventoryItemQuantitySet.subscribe((obj) => {
      console.log(`Item name: ${obj.itemName}`);
      console.log(`Item quantity: ${obj.quantity}`);
    });
  }
}
