import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection = new signalR.HubConnectionBuilder().withUrl('https://localhost:7221/events').build();

    public startConnection = () => {
      console.log("starting connection");
      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }

    public addTransferChartDataListener = () => {
      this.hubConnection.on('InventoryItemSet', (data) => {
        console.log(data);
      });
    }
}
