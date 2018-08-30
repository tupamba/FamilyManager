import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../service/authentication.service';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styles: []
})
export class PageComponent implements OnInit {

  constructor(private atuhService:AuthenticationService) { }

  ngOnInit() {
  }

}
