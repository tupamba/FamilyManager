import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { AppConfig } from './app.config.service';
import { ConfirmInvitation } from '../_models/groupFamily/groupFamily-Model';
import { map } from 'rxjs/operators';
import { GlobalService } from './global.service';

@Injectable({
  providedIn: 'root'
})
export class GroupFamilyService extends GlobalService {

  constructor(http: Http, config: AppConfig) {
    super(http, config);
   }
  aceptInvitation(familyName: ConfirmInvitation) {
     let url = this.config.apiUrl + '/api/Family/ConfirmUsertoFamily';
     let data = familyName;
     return this.http.post(url,data,this.jwt()).pipe();
   }
}
