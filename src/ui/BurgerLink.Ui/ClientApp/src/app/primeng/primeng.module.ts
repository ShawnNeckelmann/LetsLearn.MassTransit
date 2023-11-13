import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { SplitterModule } from 'primeng/splitter';
import { ListboxModule } from 'primeng/listbox';
import { DividerModule } from 'primeng/divider';
import { ToastModule } from 'primeng/toast';
import { DropdownModule } from 'primeng/dropdown';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ButtonModule,
    DividerModule,
    DropdownModule,
    ListboxModule,
    SplitterModule,
    TableModule,
    ToastModule,
  ],
  exports: [
    ButtonModule,
    DividerModule,
    DropdownModule,
    ListboxModule,
    SplitterModule,
    TableModule,
    ToastModule,
  ],
})
export class PrimengModule {}
