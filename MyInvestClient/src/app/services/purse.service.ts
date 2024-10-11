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

  create(body: any): Observable<any>
  {
    const headers = this.authService.getHeaders();
    return this.http.post(this.url, body, {headers, observe: 'response'});
  }

  delete(id: any): Observable<any>
  {
    const headers = this.authService.getHeaders();
    const urlForRequest = this.url + "/" + id;
    return this.http.delete(urlForRequest, {headers, observe: 'response'});
  }

  getById(id: any): Observable<any>
  {
    const headers = this.authService.getHeaders();
    const urlForRequest = this.url + "/" + id;
    return this.http.get(urlForRequest, {headers, observe: 'response' });
  }

  update(body: any): Observable<any>
  {
    const headers = this.authService.getHeaders();
    const urlForRequest = this.url + "/" + body.user_Id;
    return this.http.put(urlForRequest, body, {headers, observe: 'response'});
  }
}
