import { Routes, RouterModule } from '@angular/router';
//import { HomeComponent } from './home/index';
import { LoginComponent } from './profile/login/login.component';
import { RegisterComponent } from './profile/register/register.component';
import { NgModule } from '@angular/core';

 
const appRoutes: Routes = [
    // { path: 'confirmInvitation', component: ConfirmInvitationComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
 
    // otherwise redirect to home
    { path: '', redirectTo: '/login', pathMatch: 'full'  }
];
 
//export const routing = RouterModule.forRoot(appRoutes);
@NgModule({
    imports: [
      RouterModule.forRoot(
        appRoutes,
        { enableTracing: true } // <-- debugging purposes only
      )
    ],
    exports: [
      RouterModule
    ]
  })//
  export class AppRoutingModule { }