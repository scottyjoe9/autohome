import { Injectable, NgZone } from '@angular/core';
import { User } from './user';
import { Platform } from '@ionic/angular';
import { IAuthPlugin } from './i.auth.plugin';
import { WebAuthPlugin } from './web.auth.plugin';
import { SimpleEventDispatcher,ISimpleEvent } from 'ste-simple-events';
import { delay } from 'q';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authPlugin: IAuthPlugin;
  private initialised: boolean = false;
  private isAuthorisedEvent: SimpleEventDispatcher<boolean> = new SimpleEventDispatcher<boolean>();
  private userUpdatedSubscription = (u: User) => this.userUpdated(u);

  public isAuthorised: boolean = false;
  public currentUser: User = new User();
  public onIsAuthorised: ISimpleEvent<boolean> = this.isAuthorisedEvent.asEvent();

  constructor(private platform: Platform,
    private zone: NgZone) {

  }

  public initialise(){
    console.log("auth.service: initialise()");

    this.initialised = true;

    if(this.platform.is('cordova'))
    {
      console.log("auth.service.initialise(): is cordova (mobile app)");
    }
    else if (this.platform.is('electron'))
    {
      console.log("auth.service.initialise(): is electron (desktop app)")
    }
    else
    {
      console.log("auth.service.initialise(): is web app");
      this.authPlugin = new WebAuthPlugin();
    }

    this.authPlugin.onUserUpdated.subscribe(this.userUpdatedSubscription);
  }

  public async logout(){
    this.authPlugin.logout();
    this.authPlugin.onUserUpdated.unsubscribe(this.userUpdatedSubscription);
    this.authPlugin = null;
    this.userUpdated(new User());
    console.log("Logged out, waiting 500 ms")
    await delay(500);
    console.log("Finished waiting");
  }

  private userUpdated(user: User){
    this.currentUser = user;
    this.isAuthorised = user !== null;
    this.zone.run(() => {
      this.isAuthorisedEvent.dispatch(this.isAuthorised);
    })
  }
}
