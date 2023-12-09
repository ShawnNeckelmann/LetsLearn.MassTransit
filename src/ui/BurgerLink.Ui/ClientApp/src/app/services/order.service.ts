import { Inject, Injectable, Signal, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private _orderState = signal<Order[]>([]);

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    console.log('BASE_URL', baseUrl);

    this.http
      .get<AllOrders>(`${this.baseUrl}api/order/all`)
      .subscribe((results: AllOrders) => {
        this._orderState.update((x) => {
          return results.orders;
        });
      });
  }

  public createOrder(orderName: string): Observable<Order> {
    const createOrder: Order = {
      orderName: orderName,
      orderItemIds: [],
      id: '',
      confirmationStatus: '',
    };

    return this.http.post<Order>(`${this.baseUrl}api/order`, createOrder);
  }

  public getOrder(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}api/order?orderId=${orderId}`);
  }

  get getAllOrders(): Signal<Order[]> {
    return this._orderState.asReadonly();
  }

  public setOrderItems(orderId: string, items: string[]): Observable<Order> {
    const putItem: Order = {
      confirmationStatus: '',
      id: orderId,
      orderItemIds: items,
      orderName: '',
    };

    console.log(putItem);

    return this.http.put<Order>(`${this.baseUrl}api/order`, putItem);
  }
}

export interface Order {
  confirmationStatus: string;
  id: string;
  orderItemIds: string[];
  orderName: string;
}

export interface AllOrders {
  count: number;
  orders: Order[];
}
