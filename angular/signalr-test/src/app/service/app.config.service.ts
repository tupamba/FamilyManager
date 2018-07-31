import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppConfig {
  public readonly apiUrl: string = "http://localhost:3460";
}
