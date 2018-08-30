import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GroupFamilyService } from '../../../service/groupfamily.service';
import { AlertService } from '../../../service/alert.service';
import { ConfirmInvitation } from '../../../_models/groupFamily/groupFamily-Model';


@Component({
  selector: 'app-confirm-invitation',
  templateUrl: './confirm-invitation.component.html',
  styles: []
})
export class ConfirmInvitationComponent implements OnInit, OnDestroy {
  private family:string;
  private suscribe:any;
  private loading = false;
  constructor(private route: ActivatedRoute, 
    private service:GroupFamilyService,
    private alertService: AlertService) { }

  ngOnInit() {
    this.suscribe = this.route
      .queryParams
      .subscribe(params => {
        // Defaults to 0 if no query param provided.
        this.family = params['family'] || "";
      });
  }
  ngOnDestroy() {
    if (this.suscribe)
      this.suscribe.unsubscribe();
  }
  next()
  {
    this.loading = true;
    let data:ConfirmInvitation = new ConfirmInvitation();
    data.FamilyName = this.family;
    this.service.aceptInvitation(data).subscribe(
      data => {
        this.alertService.success("Confirmación enviada correctamente.");
        this.loading = false;
      },
      error => {
        this.alertService.error(error._body);
        this.loading = false;
      }
    );
  }

}
