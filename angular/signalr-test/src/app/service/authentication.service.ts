import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppConfig } from './app.config.service';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: Http, private config: AppConfig) { }
  createAuthorizationHeader(headers: Headers, token: string) {
    headers.append('Authorization', 'Bearer ' + token);
  }
  createHeaderContentType(headers: Headers, content: string) {
    headers.append('Content-Type', content);
  }
  login(username: string, password: string) {
   // let headers = new Headers();
    //this.createHeaderContentType(headers, 'application/x-www-form-urlencoded');
    let url = this.config.apiUrl + '/Token';
    let data = 'grant_type=password&username=' + username + '&password=' + password;
    let headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded'});
    let options = new RequestOptions({ headers: headers });
    return this.http.post(url,data,options).pipe(map(response => {
        // login successful if there's a jwt token in the response
        let user = response.json();
        if (user && user.access_token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
        }
      }));
  }
  validateGroup(name) {
    return this.http.post(this.config.apiUrl + '/Token', name).pipe(map((response: Response) => {
      return response;
    }));
  }
  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
  }
}
