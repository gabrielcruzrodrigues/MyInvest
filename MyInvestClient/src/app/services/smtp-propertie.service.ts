import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'environments/environment.prod';
import { catchError, Observable, throwError } from 'rxjs';
import { AppService } from './app.service';
import { CreateSmtpPropertie } from '@/types/CreateSmtpPropertie';
import { error } from 'console';

@Injectable({
  providedIn: 'root'
})
export class SmtpPropertieService {

  url: string = environment.URL + '/SmtpProperties/';

  constructor(
    private http: HttpClient,
    private route: Router,
    private appService: AppService
  ) { }

  getAllSmtpProperties(): Observable<any> {
    const headers = this.appService.getHeaders();
    return this.http.get(this.url, { headers, observe: 'response' }).pipe(
      catchError((error) => {
        console.error('Error getting all smtpProperties', error);
        return throwError(() => error);
      })
    )
  }

  createSmtpProfile(data: CreateSmtpPropertie): Observable<any> {
    const headers = this.appService.getHeaders();
    return this.http.post(this.url, data, { headers, observe: 'response' }).pipe(
      catchError((error) => {
        console.error('Error creating SMTP profile:', error);
        return throwError(() => error);
      })
    )
  }
}
