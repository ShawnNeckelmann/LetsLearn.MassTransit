import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Order, OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-new-order',
  templateUrl: './new-order.component.html',
  styleUrl: './new-order.component.css',
})
export class NewOrderComponent {
  public orderName: string = 'test';

  constructor(
    private serviceOrder: OrderService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  public onSubmit() {
    this.serviceOrder.createOrder(this.orderName).subscribe((obj: Order) => {
      let json = JSON.stringify(obj);
      console.log(json);

      this.router.navigate(['../edit', obj.id], {
        relativeTo: this.route,
      });
    });
  }
}
