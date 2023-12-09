import { Observable } from 'rxjs';
import {
  ExternalsService,
  ExternalSite,
} from './../services/externals.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-externals',
  templateUrl: './externals.component.html',
  styleUrls: ['./externals.component.css'],
})
export class ExternalsComponent {
  products$: Observable<ExternalSite[]>;

  constructor(public externals: ExternalsService) {
    this.products$ = externals.Sites();
  }
}
