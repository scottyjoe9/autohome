import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/authorisation/auth.service';
import { NavController } from '@ionic/angular';

declare var gapi:any;

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
})
export class LoginPage implements OnInit {

  constructor(private authService: AuthService,
    private nav: NavController) {
      this.authService.onIsAuthorised.subscribe(a => this.authorisationChanged(a));
  }

  ngOnInit(){
    this.authService.initialise();
  }

  private authorisationChanged(authorised: boolean){
    console.log("login.page.authorisationChanged: authorised: " + authorised);

    if (authorised)
      this.nav.navigateRoot('master');
  }
}
