import { Component,OnInit } from '@angular/core';
import { Observable } from "rxjs";
// import { ChannelService, ConnectionState } from './service/channel.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  connectionState$: Observable<string>;
  constructor(
   // private channelService: ChannelService
  ) {

    // Let's wire up to the signalr observables
    //
    // this.connectionState$ = this.channelService.connectionState$
    //   .pipe(map((state: ConnectionState) => { return ConnectionState[state]; }));

    // this.channelService.error$.subscribe(
    //   (error: any) => { console.warn(error); },
    //   (error: any) => { console.error("errors$ error", error); }
    // );

    // Wire up a handler for the starting$ observable to log the
    //  success/fail result
    //
    // this.channelService.starting$.subscribe(
    //   () => { console.log("signalr service has been started"); },
    //   () => { console.warn("signalr service failed to start!"); }
    // );
  }
  ngOnInit() {
    // Start the connection up!
    //
   // console.log("Starting the channel service");

   // this.channelService.start();
  }
}
