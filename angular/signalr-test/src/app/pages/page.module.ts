import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { GlobalService } from '../service/global.service';
import { PageComponent } from './page.component';
import { AlertComponent } from '../_directives/alert/alert.component';
import { PageRoutes } from './page.routes';
import { GroupFamilyService } from '../service/groupfamily.service';
import { ConfirmInvitationComponent } from './groupFamily/confirm-invitation/confirm-invitation.component';
import { HeaderComponent } from '../shared/header/header.component';
import { SidebarComponent } from '../shared/sidebar/sidebar/sidebar.component';
import { BreadcrumsComponent } from '../shared/breadcrums/breadcrums/breadcrums.component';

@NgModule({
    declarations:[
        ConfirmInvitationComponent,
        PageComponent,
        AlertComponent,
        HeaderComponent,
        SidebarComponent,
        BreadcrumsComponent
    ],
    exports:[
        ConfirmInvitationComponent,
        AlertComponent
    ],
    providers:
    [
        GroupFamilyService,
        GlobalService
    ],
    imports:[
        BrowserModule,
        PageRoutes
    ]

})
export class PageModule{}