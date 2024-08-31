import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient
  ) { }

  url: string = development_environments.url + "/User";

  create(data: any): Observable<any>
  {
    const urlForRequest = this.url + "/user"
    return this.http.post(this.url, data, {observe: 'response'});
  }

}
