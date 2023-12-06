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
      orderId: '',
    };

    return this.http.post<Order>(`${this.baseUrl}api/order`, createOrder);
  }

  public getOrder(orderId: string): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}api/order?orderId=${orderId}`);
  }

  public getOrders(): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}api/order`);
  }
}

export interface Order {
  orderName: string;
  orderItemIds: string[];
  orderId: string;
}
