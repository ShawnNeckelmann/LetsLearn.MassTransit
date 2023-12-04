import { Router } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-name-order',
  templateUrl: './name-order.component.html',
  styleUrl: './name-order.component.css',
})
export class NameOrderComponent {
  orderName: string = '';
  disabledSubmit: boolean = true;

  constructor(private router: Router) {}

  onSubmit() {}

  onKeyPress() {
    const length = this.orderName.length;
    this.disabledSubmit = !(length > 0);
  }
}
