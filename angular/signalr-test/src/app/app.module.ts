import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule }     from '@angular/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule }   from '@angular/forms';

import { map } from 'rxjs/operators';

//import { ChannelService, ChannelConfig, SignalrWindow } from './service/channel.service';

import { AppComponent } from './app.component';
import { TaskComponent } from './task/task.component';
import { AppConfig } from './service/app.config.service';
import { AppRoutingModule } from './app.routing';
import { AuthGuard } from './service/_guards/auth.guard.service';
import { AuthenticationService } from './service/authentication.service';
import { UserService } from './service/user.service';
import { AlertService } from './service/alert.service';
import { LoginComponent } from './profile/login/login.component';
import { RegisterComponent } from './profile/register/register.component';
import { EqualValidator } from './validators/validators';
import { GlobalService } from './service/global.service';
import { PageModule } from './pages/page.module';
import { TranslatePipe } from './pipe/lang/translate.pipe';
import { TranslateService } from './service/lang/translate.service';
import { ResetPasswordComponent } from './profile/reset-password/reset-password.component';



// let channelConfig = new ChannelConfig();  
// channelConfig.url = "http://localhost:7331/signalr";  
// channelConfig.hubName = "EventHub";
@NgModule({
  declarations: [
    AppComponent,
    TaskComponent,
    LoginComponent,
    RegisterComponent,
    EqualValidator,
    TranslatePipe,
    ResetPasswordComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    ReactiveFormsModule,
    FormsModule,
    PageModule,
    AppRoutingModule
  ],
  providers: [
    AppConfig,
   // ChannelService,
    AuthGuard,
    AuthenticationService,
    UserService,
    AlertService,
    GlobalService,
    TranslateService
    // { provide: SignalrWindow, useValue: window },
    // { provide: 'channel.config', useValue: channelConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
