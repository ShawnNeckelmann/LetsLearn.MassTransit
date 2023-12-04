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

  public createOrder(orderName: string, ids: string[]): Observable<string> {
    const createOrder: CreateOrder = {
      orderName: orderName,
      orderItemIds: ids,
      orderId: this.createGUID(),
    };

    return this.http
      .post<CreateOrder>(`${this.baseUrl}api/order`, createOrder)
      .pipe(
        map((x) => {
          return createOrder.orderId;
        })
      );
  }

  private createGUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(
      /[xy]/g,
      function (c) {
        const r = (Math.random() * 16) | 0;
        const v = c === 'x' ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      }
    );
  }
}

export interface CreateOrder {
  orderName: string;
  orderItemIds: string[];
  orderId: string;
}
