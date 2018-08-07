import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private subject = new Subject<any>();
  private keepAfterNavigationChange = false;

  constructor(private router: Router) {
    // clear alert message on route change
    router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        if (this.keepAfterNavigationChange) {
          // only keep for a single location change
          this.keepAfterNavigationChange = false;
        } else {
          // clear alert
          this.subject.next();
        }
      }
    });
  }

  success(message: string, keepAfterNavigationChange = false) {
    this.keepAfterNavigationChange = keepAfterNavigationChange;
    this.subject.next({ type: 'success', text: message });
  }

  error(message: string, keepAfterNavigationChange = false) {
    this.keepAfterNavigationChange = keepAfterNavigationChange;
    let error = "No se puede procesar tu solicitud";
    try {
      let response = JSON.parse(message);
      if(response.ModelState)
      {
      let pp = Object.keys(response.ModelState).map((key)=>{ return response.ModelState[key]});
      pp.forEach(element => {
        error += element;
      });
      }else if(response.error_description)
       error = response.error_description;
    } catch (error) {
      
    }

    this.subject.next({ type: 'error', text: error });
  }

  getMessage(): Observable<any> {
    return this.subject.asObservable();
  }
}
