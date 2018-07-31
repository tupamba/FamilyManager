import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from './app.config.service';


@Injectable()
export class GlobalService {
  constructor(public http: Http,public config: AppConfig) { }

  // private helper methods

  public jwt() {
    // create authorization header with jwt token
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.access_token) {
      let headers = new Headers({  'Content-Type': 'application/json','Authorization': 'Bearer ' + currentUser.access_token });
      return new RequestOptions({ headers: headers });
    }
  }
}