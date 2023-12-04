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
import { PrimengModule } from './primeng/primeng.module';
import { MessageService } from 'primeng/api';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ManageOrdersComponent } from './orders/manage-orders/manage-orders.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    ExternalsComponent,
    ManageOrdersComponent,
    InventoryComponent,
    MenuComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    PrimengModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', pathMatch: 'full', redirectTo: '/inventory' },
      { path: 'inventory', component: InventoryComponent },
      { path: 'menu', component: MenuComponent },
      {
        path: 'orders/manage',
        children: [
          {
            component: ManageOrdersComponent,
            path: '',
          },
        ],
      },
      { path: 'externals', component: ExternalsComponent },
    ]),
  ],
  providers: [MessageService],
  bootstrap: [AppComponent],
})
export class AppModule {}
