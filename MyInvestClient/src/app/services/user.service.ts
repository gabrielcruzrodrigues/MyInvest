import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment.prod';
import { AppService } from './app.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient,
    private appService: AppService
  ) { }

  url: string = environment.URL + "/user";
  headers = this.appService.getHeaders();
  
  getPurses(userId: any): Observable<any>
  {
    const urlForRequest = this.url + `/${userId}/purses`;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response'});
  }

}
