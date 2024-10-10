import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { development_environments } from '../environments/development-environments';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  url: string = development_environments.url + "/user";
  headers = this.authService.getHeaders();
  
  getPurses(userId: any): Observable<any>
  {
    const urlForRequest = this.url + `/${userId}/purses`;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response'});
  }
}
