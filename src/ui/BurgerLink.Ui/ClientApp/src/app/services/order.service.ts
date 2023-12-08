import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {}

  public createOrder(orderName: string): Observable<Order> {
    const createOrder: Order = {
      orderName: orderName,
      orderItemIds: [],
      id: '',
    };

    return this.http.post<Order>(`${this.baseUrl}api/order`, createOrder);
  }

  public getOrder(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}api/order?orderId=${orderId}`);
  }

  public getOrders(): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}api/order/all`);
  }

  public setOrderItems(orderId: string, items: string[]): Observable<Order> {
    const putItem = {
      orderId: orderId,
      inventoryIds: items,
    };

    console.log(putItem);

    return this.http.put<Order>(`${this.baseUrl}api/order/items`, putItem);
  }
}

export interface Order {
  orderName: string;
  orderItemIds: string[];
  id: string;
}
