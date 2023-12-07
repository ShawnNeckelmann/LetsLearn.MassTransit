import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Signal, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class InventoryService {
  private hubConnection: HubConnection;
  private _inventoryState = signal<InventoryItem[]>([]);

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
        this._inventoryState.update((x) => {
          const retval: InventoryItem[] = [];
          results.inventoryItems.forEach((item) => {
            retval.push(item);
          });

          return retval;
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
    return this._inventoryState.asReadonly();
  }

  public AddInventoryItem(
    name: string,
    quantity: number = 1
  ): Observable<number> {
    return this.http.post<number>(this.baseUrl + 'api/inventory', {
      itemName: name,
      quantity: quantity,
    });
  }

  public ModifyInventoryItem(
    id: string,
    name: string,
    quantity: number
  ): Observable<number> {
    return this.http.put<number>(this.baseUrl + 'api/inventory', {
      id: id,
      itemName: name,
      quantity: quantity,
    });
  }

  private configureHubEvents() {
    this.hubConnection.on('inventoryItemAdded', (data: InventoryItem) => {
      console.log('an item has been added to inventory');

      this._inventoryState.update((value: InventoryItem[]) => {
        value.push(data);
        return value;
      });
    });

    this.hubConnection.on('inventoryItemModified', (data: InventoryItem) => {
      this._inventoryState.update((value: InventoryItem[]) => {
        const index = value.findIndex((x) => x.id == data.id);
        if (index == -1) {
          value.push(data);
        } else {
          value[index] = data;
        }

        return value;
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
