import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { Order, OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-new-order-2',
  templateUrl: './new-order-2.component.html',
  styleUrl: './new-order-2.component.css',
})
export class NewOrder2Component implements OnInit {
  public order$: Observable<Order>;
  public order: Order;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serviceOrder: OrderService
  ) {
    this.order = {
      orderName: '',
      orderItemIds: [],
      orderId: '',
    };

    this.order$ = new BehaviorSubject<Order>(this.order);
  }

  ngOnInit(): void {
    const idOrder = this.route.snapshot.paramMap.get('id');
    if (idOrder == null) {
      console.log(this.route);

      this.router.navigate(['../orders'], {
        relativeTo: this.route,
      });
      return;
    }

    this.order$ = this.serviceOrder.getOrder(idOrder).pipe(
      tap((order: Order) => {
        this.order = order;
      })
    );

    this.order$.subscribe();
  }
}
