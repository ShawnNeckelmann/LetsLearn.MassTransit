import { Component, OnInit, Signal, effect } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import {
  InventoryItem,
  InventoryService,
} from 'src/app/services/inventory.service';
import { Order, OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-new-order-2',
  templateUrl: './new-order-2.component.html',
  styleUrl: './new-order-2.component.css',
})
export class NewOrder2Component implements OnInit {
  public order$: Observable<Order>;
  public order: Order;
  public availableItems$: Signal<InventoryItem[]>;
  public selectedItems: Order[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private serviceOrder: OrderService,
    serviceInventory: InventoryService
  ) {
    this.availableItems$ = serviceInventory.InventoryItems;
    this.order = {
      orderName: '',
      orderItemIds: [],
      orderId: '',
    };
    this.order$ = new BehaviorSubject<Order>(this.order);

    effect(() => {
      this.availableItems$ = serviceInventory.InventoryItems;
    });
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

  onSave() {
    console.log(this.selectedItems);
  }
}
