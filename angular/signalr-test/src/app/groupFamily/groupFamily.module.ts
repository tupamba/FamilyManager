import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { ConfirmInvitationComponent } from '../groupFamily/confirm-invitation/confirm-invitation.component';
import { GroupFamilyService } from '../groupFamily/service/groupfamily.service';
import { GlobalService } from '../service/global.service';
import { GroupFamilyComponent } from './group-family.component';
import { PAGE_ROUTES } from './groupFamily.routes';
import { AlertComponent } from '../_directives/alert/alert.component';

@NgModule({
    declarations:[
        ConfirmInvitationComponent,
        GroupFamilyComponent,
        AlertComponent
    ],
    exports:[
        ConfirmInvitationComponent
    ],
    providers:
    [
        GroupFamilyService,
        GlobalService
    ],
    imports:[
        BrowserModule,
        PAGE_ROUTES
    ]

})
export class GroupFamilyModule{}