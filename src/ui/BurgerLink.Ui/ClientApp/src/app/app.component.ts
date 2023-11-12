import { Component } from '@angular/core';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'app';

  constructor(public signalRService: SignalrService) {}

  ngOnInit() {
    this.signalRService.OnInventoryItemQuantitySet.subscribe((obj) => {
      console.log(`Item name: ${obj.itemName}`);
      console.log(`Item quantity: ${obj.quantity}`);
    });
  }
}
