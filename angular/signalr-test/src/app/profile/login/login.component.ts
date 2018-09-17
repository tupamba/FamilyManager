import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";

import { AuthenticationService } from "../../service/authentication.service";
import { AlertService } from "../../service/alert.service";
import { UserService } from "../../service/user.service";
import { TranslateService } from "../../service/lang/translate.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html"
})
export class LoginComponent implements OnInit {
  model: any = {};
  loading = false;
  returnUrl: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private alertService: AlertService,
    private userService: UserService,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();

    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
  }
  login() {
    this.loading = true;
    this.authenticationService
      .login(this.model.username, this.model.password)
      .subscribe(
        data => {
          this.router.navigate([this.returnUrl]);
          this.loading = false;
        },
        error => {
          this.alertService.error(error._body);
          this.loading = false;
        }
      );
  }
  forGot() {
    if (confirm(this.translate.instant("Are you sure to reset password"))) {
      if (this.model || this.model.username) {
        this.loading = true;
        let reset = { 'Email': this.model.username };
        this.userService.forGotPassword(reset)
          .subscribe(
            data => {
              this.alertService.success('Reset successful, check your mail', true);
              this.router.navigate(['/resetPassword']);
              this.loading = false;
            },
            error => {
              this.alertService.error(error._body);
              this.loading = false;
            });
      } else
        this.alertService.error_online('Ingresá tu mail por favor');
    }
  }
}
