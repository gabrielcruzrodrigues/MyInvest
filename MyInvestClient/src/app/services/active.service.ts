import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActiveService {

  url: string = development_environments.url;

  constructor(
    private http: HttpClient
  ) { }

  search(active: string): Observable<any>
  {
    const urlForRequest = this.url + `/search-active/${active}`;
    return this.http.get(urlForRequest, { observe: 'response' });
  }
}
