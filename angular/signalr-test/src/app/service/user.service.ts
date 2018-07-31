import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from './app.config.service';
import { User } from '../_models/user/user.component';
import { map } from 'rxjs/operators';

@Injectable()
export class UserService {
  constructor(private http: Http, private config: AppConfig) { }

  getAll() {
    return this.http.get(this.config.apiUrl + '/users', this.jwt()).pipe(map((response: Response) => response.json()));
  }

  getById(_id: string) {
    return this.http.get(this.config.apiUrl + '/users/' + _id, this.jwt()).pipe(map((response: Response) => response.json()));
  }

  create(user: User) {
    return this.http.post(this.config.apiUrl + '/Account/Register', user);
  }

  update(user: User) {
    return this.http.put(this.config.apiUrl + '/users/' + user._id, user, this.jwt());
  }

  delete(_id: string) {
    return this.http.delete(this.config.apiUrl + '/users/' + _id, this.jwt());
  }

  // private helper methods

  private jwt() {
    // create authorization header with jwt token
    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && currentUser.access_token) {
      let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.access_token });
      return new RequestOptions({ headers: headers });
    }
  }
}