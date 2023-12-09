import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ExternalsService {
  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient
  ) {}

  public Sites(): Observable<ExternalSite[]> {
    return this.http
      .get<ExternalsResponse>(this.baseUrl + 'api/externals')
      .pipe(map((x) => x.externals));
  }
}

export interface ExternalsResponse {
  externals: ExternalSite[];
}

export interface ExternalSite {
  name: string;
  address: string;
  credentials: string;
  information: string;
}
