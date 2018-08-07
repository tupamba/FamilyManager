import { RouterModule, Routes } from '@angular/router';
import { ConfirmInvitationComponent } from '../groupFamily/confirm-invitation/confirm-invitation.component';
import { GroupFamilyComponent } from './group-family.component';
import { AuthGuard } from '../service/_guards/auth.guard.service';


const pagesRoutes: Routes = [
  {
    path: '',
    component: GroupFamilyComponent,
    children: [
      { path: 'confirmInvitation', component: ConfirmInvitationComponent },
      { path: '', redirectTo: '/confirmInvitation', pathMatch: 'full' }
    ], canActivate: [AuthGuard] 
  }
]
export const PAGE_ROUTES = RouterModule.forChild(pagesRoutes);
export class GroupFamilyRoutes { }