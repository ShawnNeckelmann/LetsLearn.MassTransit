import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Signal, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private hubConnection: HubConnection;
  private _backingInventory: Map<string, InventoryItem>;
  private _inventoryItems = signal<InventoryItem[]>([]);

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7221/events')
      .build();

    this.startConnection();
    this._backingInventory = new Map<string, InventoryItem>();

    this.http
      .get<InventoryResponse>(this.baseUrl + 'api/inventory')
      .subscribe((results) => {
        this._inventoryItems.mutate((x) => {
          results.inventoryItems.forEach((item) => {
            this._backingInventory.set(item.id, item);
            x.push(item);
          });
        });
      });
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));

    this.configureInventoryItemQuantitySet();
  }

  get InventoryItems(): Signal<InventoryItem[]> {
    return this._inventoryItems.asReadonly();
  }

  private configureInventoryItemQuantitySet() {
    this.hubConnection.on('inventoryItemAdded', (data) => {
      console.log('added: ' + JSON.stringify(data));
    });

    this.hubConnection.on('inventoryItemModified', (data) => {
      console.log('modified: ' + JSON.stringify(data));
    });
  }
}

export interface InventoryResponse {
  inventoryItems: InventoryItem[];
}

export interface InventoryItem {
  id: string;
  name: string;
  quantity: number;
}
