import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private eventSubject = new Subject<InventoryItemQuantitySet>();
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7221/events')
      .build();

    this.startConnection();
    this.configureInventoryItemQuantitySet();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  private configureInventoryItemQuantitySet() {
    this.hubConnection.on('InventoryItemQuantitySet', (data) => {
      var obj = data as InventoryItemQuantitySet;
      this.eventSubject.next(obj);
    });
  }

  get OnInventoryItemQuantitySet() {
    return this.eventSubject.asObservable();
  }
}

export interface InventoryItemQuantitySet {
  itemName: string;
  quantity: number;
}
