import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../service/_guards/auth.guard.service';
import { NgModule } from '@angular/core';
import { PageComponent } from './page.component';
import { ConfirmInvitationComponent } from './groupFamily/confirm-invitation/confirm-invitation.component';


const pagesRoutes: Routes = [
  {
    path: '',
    component: PageComponent,
    children: [
      { path: 'confirmInvitation', component: ConfirmInvitationComponent }
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