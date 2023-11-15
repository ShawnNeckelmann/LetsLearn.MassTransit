import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ExternalsService {
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {}

  public Sites(): Observable<ExternalSite[]> {
    console.log('BASE URL' + this.baseUrl);
    return this.http.get<ExternalSite[]>(this.baseUrl + 'externals');
  }
}

export interface ExternalSite {
  name: string;
  address: string;
  credentials: string;
  information: string;
}
