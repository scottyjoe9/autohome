import { Component } from '@angular/core';

import { Platform, NavController } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';
import { AuthService } from '../services/authorisation/auth.service';

@Component({
  selector: 'master',
  templateUrl: 'master.page.html',
  styleUrls: ['master.page.scss'],
})
export class MasterPage {
  public appPages = [
    {
      title: 'Home',
      url: 'home',
      icon: 'home'
    },
    {
      title: 'List',
      url: 'list',
      icon: 'list'
    }
  ];

  constructor(private auth: AuthService,
    private nav: NavController) {
    }

  async logout(){
    await this.auth.logout();

    this.nav.navigateRoot('login');
  }
}
