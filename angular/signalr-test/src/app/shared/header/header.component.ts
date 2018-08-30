import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../service/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  constructor(private authService:AuthenticationService, private navigation:Router ) { }

  ngOnInit() {
  }
  logout()
  {
    this.authService.logout();
    this.navigation.navigateByUrl('/login');
  }
}
