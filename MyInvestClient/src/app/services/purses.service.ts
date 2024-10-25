import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment.prod';
import { Observable } from 'rxjs';
import { AppService } from './app.service';

@Injectable({
  providedIn: 'root'
})
export class PursesService {

  constructor(
    private http: HttpClient,
    private appService: AppService
  ) { }

  url: string = environment.URL + "/purse";

  create(body: any): Observable<any>
  {
    const headers = this.appService.getHeaders();
    return this.http.post(this.url, body, {headers, observe: 'response'});
  }

  delete(id: any): Observable<any>
  {
    const headers = this.appService.getHeaders();
    const urlForRequest = this.url + "/" + id;
    return this.http.delete(urlForRequest, {headers, observe: 'response'});
  }

  getById(id: any): Observable<any>
  {
    const headers = this.appService.getHeaders();
    const urlForRequest = this.url + "/" + id;
    return this.http.get(urlForRequest, {headers, observe: 'response' });
  }

  update(body: any): Observable<any>
  {
    const headers = this.appService.getHeaders();
    const urlForRequest = this.url + "/" + body.user_Id;
    return this.http.put(urlForRequest, body, {headers, observe: 'response'});
  }

}
