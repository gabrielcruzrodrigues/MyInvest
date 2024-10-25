import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppService } from './app.service';
import { environment } from 'environments/environment.prod';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ActiveService {

  url: string = environment.URL;
  headers = this.appService.getHeaders();

  constructor(
    private http: HttpClient, private appService: AppService
  ) { }


  search(active: string, percentage: number): Observable<any>
  {
    const urlForRequest = this.url + `/search-active/${active}/${percentage}`;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response' });
  }

  create(purseId: string, type: string, code: string, dYDesiredPercentage: string): Observable<any>
  {
    const urlForRequest = this.url + "/active";

    const objForRequest = {
      type: type,
      purse_Id: purseId,
      code: code,
      dyDesiredPercentage: dYDesiredPercentage
    }
    return this.http.post(urlForRequest, objForRequest, {headers: this.headers, observe: 'response' });
  }

  searchActivesByPurseId(purseId: string): Observable<any>
  {
    const urlForRequest = this.url + "/get-actives/" + purseId;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response' });
  }

  searchActivesForShowPurseDetails(purseId: string): Observable<any>
  {
    const urlForRequest = this.url + "/search-active-purse-details/" + purseId;
    return this.http.get(urlForRequest, {headers: this.headers, observe: 'response' });
  }

  delete(purseId: string): Observable<any>
  {
    const urlForRequest = this.url + "/active/" + purseId;
    return this.http.delete(urlForRequest, {headers: this.headers, observe: 'response' });
  }

  update(activeId: string, dYDesiredPercentage: number): Observable<any>
  {
    const urlForRequest = this.url + "/active/" + activeId;

    const objForRequest = {
      dyDesiredPercentage: dYDesiredPercentage
    }
    return this.http.put(urlForRequest, objForRequest, {headers: this.headers, observe: 'response' });
  }

}