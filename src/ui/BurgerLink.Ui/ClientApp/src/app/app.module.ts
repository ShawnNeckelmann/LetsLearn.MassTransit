import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

import { ExternalsComponent } from './externals/externals.component';
import { InventoryComponent } from './inventory/inventory.component';
import { MenuComponent } from './menu/menu.component';
import { OrdersComponent } from './orders/orders.component';
import { PrimengModule } from './primeng/primeng.module';
import { MessageService } from 'primeng/api';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ExternalsComponent,
    InventoryComponent,
    MenuComponent,
    OrdersComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    PrimengModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: InventoryComponent, pathMatch: 'full' },
      { path: 'menu', component: MenuComponent },
      { path: 'orders', component: OrdersComponent },
      { path: 'externals', component: ExternalsComponent },
    ]),
  ],
  providers: [MessageService],
  bootstrap: [AppComponent],
})
export class AppModule {}
