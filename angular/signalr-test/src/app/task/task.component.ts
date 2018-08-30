import { Component, OnInit, Input } from "@angular/core";
import { Http, Response } from "@angular/http";
//import { ChannelService, ChannelEvent } from '../service/channel.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styles: []
})
export class TaskComponent implements OnInit {

  @Input() eventName: string;
  @Input() apiUrl: string;

  messages = "";

  private channel = "TaskChannel";

  constructor(
    private http: Http,
   // private channelService: ChannelService
  ) {

  }

  ngOnInit() {
    // Get an observable for events emitted on this channel
    //
    console.log("ngOnInit");
    // this.channelService.sub(this.channel).subscribe(
    //   (x: ChannelEvent) => {
    //     switch (x.Name) {
    //       case this.eventName: { this.appendStatusUpdate(x); }
    //     }
    //   },
    //   (error: any) => {
    //     console.warn("Attempt to join channel failed!", error);
    //   }
    // )
  }
  // private appendStatusUpdate(ev: ChannelEvent): void {
  //   // Just prepend this to the messages string shown in the textarea
  //   //
  //   console.log(ev.Data.State);
  //   let date = new Date();
  //   switch (ev.Data.State) {
  //     case "starting": {
  //       this.messages = `${date.toLocaleTimeString()} : starting\n` + this.messages;
  //       break;
  //     }

  //     case "complete": {
  //       this.messages = `${date.toLocaleTimeString()} : complete\n` + this.messages;
  //       break;
  //     }

  //     default: {
  //       this.messages = `${date.toLocaleTimeString()} : ${ev.Data.State} : ${ev.Data.PercentComplete} % complete\n` + this.messages;
  //     }
  //   }
  // }

  callApi() {
    this.http.get(this.apiUrl)
      .pipe(map((res: Response) => res.json()))
      .subscribe((message: string) => { console.log(message); });
  }

}
