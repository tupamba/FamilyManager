import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../service/_guards/auth.guard.service';
import { NgModule } from '@angular/core';
import { PageComponent } from './page.component';
import { ConfirmInvitationComponent } from './groupFamily/confirm-invitation/confirm-invitation.component';
import { ProfileComponent } from './profile/profile/profile.component';


const pagesRoutes: Routes = [
  {
    path: '',
    component: PageComponent,
    children: [
      { path: 'confirmInvitation', component: ConfirmInvitationComponent },
      { path: 'profile', component: ProfileComponent }
      // { path: '', redirectTo: '/confirmInvitation', pathMatch: 'full' }
    ], canActivate: [AuthGuard] 
  }
]
//export const PAGE_ROUTES = RouterModule.forChild(pagesRoutes);
@NgModule({
  imports: [
    RouterModule.forChild(pagesRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class PageRoutes { }