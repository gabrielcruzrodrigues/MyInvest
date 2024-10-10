import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PurseService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  url: string = development_environments.url + "/purse";
  headers = this.authService.getHeaders();

  create(body: any): Observable<any>
  {
    return this.http.post(this.url, body, {headers: this.headers, observe: 'response'});
  }

  delete(id: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + id;
    return this.http.delete(urlForRequest, {headers: this.headers, observe: 'response'});
  }

  getById(id: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + id;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response' });
  }

  update(body: any): Observable<any>
  {
    const urlForRequest = this.url + "/" + body.user_Id;
    return this.http.put(urlForRequest, body, {headers: this.headers, observe: 'response'});
  }
}
