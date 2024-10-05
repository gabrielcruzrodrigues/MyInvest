import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = 'https://localhost:7021/Auth/';
  
  constructor(private http: HttpClient) { }
  
  configureLocalStorage(body: any): void 
  {
    if (body) 
    {
      localStorage.setItem('userId', body.userId);
      localStorage.setItem('Token', body.token);
      localStorage.setItem('RefreshToken', body.refreshToken);
      localStorage.setItem('Expiration', body.expiration);
    }
  }

  verifyIfUserIdLogged() : boolean
  {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      var token = localStorage.getItem('Token');
      if (token != null) {
        return true;
      };
    }
    return false;
  }

  getId() {
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      const userId = localStorage.getItem('userId');
      return userId !== null ? userId : '';
    }
    return ''; 
  }

  createAccount(data: any) : Observable<any>
  {
    const urlForRequest = this.url + "register";
    return this.http.post(urlForRequest, data, {observe: 'response'});
  }

  login(data: any) : Observable<any>
  {
    const urlForRequest = this.url + "login";
    return this.http.post(urlForRequest, data, {observe: 'response'});
  }

  getHeaders() {
    if (typeof window == 'undefined' || typeof window.localStorage == 'undefined') {
      return;
    } 

    const token = localStorage.getItem('Token');

    if (token) {
      return new HttpHeaders().set('Authorization', `Bearer ${token}`);
    } 
    return new HttpHeaders();
  }
}
