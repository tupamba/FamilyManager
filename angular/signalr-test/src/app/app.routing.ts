import { Routes, RouterModule } from '@angular/router';
 
//import { HomeComponent } from './home/index';
import { LoginComponent } from './profile/login/login.component';
import { RegisterComponent } from './profile/register/register.component';
import { AuthGuard } from './service/_guards/auth.guard.service';
import { ConfirmInvitationComponent } from './groupFamily/confirm-invitation/confirm-invitation.component';
 
const appRoutes: Routes = [
    // { path: 'confirmInvitation', component: ConfirmInvitationComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
 
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
 
export const routing = RouterModule.forRoot(appRoutes);