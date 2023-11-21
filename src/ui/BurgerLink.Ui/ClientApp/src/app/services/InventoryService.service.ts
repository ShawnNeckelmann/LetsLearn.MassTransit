import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Signal, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private hubConnection: HubConnection;
  private _inventoryItems = signal<InventoryItem[]>([]);

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7221/events')
      .withAutomaticReconnect()
      .build();

    this.startConnection();

    this.http
      .get<InventoryResponse>(this.baseUrl + 'api/inventory')
      .subscribe((results) => {
        this._inventoryItems.mutate((x) => {
          results.inventoryItems.forEach((item) => {
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

    this.configureHubEvents();
  }

  get InventoryItems(): Signal<InventoryItem[]> {
    return this._inventoryItems.asReadonly();
  }

  private configureHubEvents() {
    this.hubConnection.on('inventoryItemAdded', (data: InventoryItem) => {
      this._inventoryItems.mutate((value: InventoryItem[]) => {
        value.push(data);
      });
    });

    this.hubConnection.on('inventoryItemModified', (data: InventoryItem) => {
      this._inventoryItems.mutate((value: InventoryItem[]) => {
        const index = value.findIndex((x) => x.id == data.id);
        if (index == -1) {
          return;
        }
        value[index] = data;
      });
    });
  }
}

export interface InventoryResponse {
  inventoryItems: InventoryItem[];
}

export interface InventoryItem {
  id: string;
  itemName: string;
  quantity: number;
}
