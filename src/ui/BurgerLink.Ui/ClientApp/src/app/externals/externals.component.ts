import { Component } from '@angular/core';

@Component({
  selector: 'app-externals',
  templateUrl: './externals.component.html',
  styleUrls: ['./externals.component.css'],
})
export class ExternalsComponent {
  products: Product[];

  constructor() {
    this.products = [];

    this.products.push({
      name: 'RabbitMq Management',
      address: 'http://localhost:15672/#/',
      information: 'Managmeent for the Rabbit Cluster',
      credentials: 'User: guest // Password: guest',
    });

    this.products.push({
      name: 'Mongo Express',
      address: 'http://localhost:8081',
      information: 'Managmeent for the Mongo Server',
      credentials: 'User: admin // Password: pass',
    });

    this.products.push({
      name: 'Grafana',
      address: 'http://localhost:3000/',
      information: 'Metrics, traces, and logs.',
      credentials: '',
    });
  }
}

interface Product {
  name: string;
  address: string;
  credentials: string;
  information: string;
}
