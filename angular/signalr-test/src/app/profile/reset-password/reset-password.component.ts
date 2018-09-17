import { Component, OnInit } from '@angular/core';
import { UserService } from '../../service/user.service';
import { AlertService } from '../../service/alert.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  model: any = {};
  loading = false;

  constructor(
    private router: Router,
    private userService: UserService,
    private alertService: AlertService) { }

  ngOnInit() {
  }
  resetPassword() {
    this.loading = true;
    this.userService.resetPassword(this.model)
      .subscribe(
        data => {
          this.alertService.success('Reset password successful', true);
          this.router.navigate(['/login']);
          this.loading = false;
        },
        error => {
          this.alertService.error(error._body);
          this.loading = false;
        });
  }
}
