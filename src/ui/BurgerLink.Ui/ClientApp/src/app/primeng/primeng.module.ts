import { NgModule } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { DividerModule } from 'primeng/divider';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ListboxModule } from 'primeng/listbox';
import { SplitterModule } from 'primeng/splitter';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';

@NgModule({
  declarations: [],
  imports: [
    ButtonModule,
    CommonModule,
    DividerModule,
    DropdownModule,
    InputTextModule,
    ListboxModule,
    SplitterModule,
    TableModule,
    ToastModule,
  ],
  exports: [
    ButtonModule,
    DividerModule,
    DropdownModule,
    InputTextModule,
    ListboxModule,
    SplitterModule,
    TableModule,
    ToastModule,
  ],
})
export class PrimengModule {}
