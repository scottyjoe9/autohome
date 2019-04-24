import { SimpleEventDispatcher, ISimpleEvent } from 'ste-simple-events';
import { User } from './user';
import { IAuthPlugin } from './i.auth.plugin';

declare var gapi: any;

export class WebAuthPlugin implements IAuthPlugin {
    private _script: HTMLScriptElement;
    private _gauth: any;
    private _userUpdated = new SimpleEventDispatcher<User>();
    private _googleLoginEventListener = (e:Event) => this.addGoogleLoginEventListener(e);
    private _gapiLoad = () => this.onGapiAuthLoaded();
    private _gapiError = () => this.onGapiAuthError();
    private _gapiTimeout = () => this.onGapiAuthTimeout();

    constructor(){
        this._script = document.createElement('script');
        this._script.src = 'https://apis.google.com/js/platform.js';
        this._script.addEventListener("load", this._googleLoginEventListener);
        document.body.appendChild(this._script);
    }

    logout(){
        console.log("WebAuthPlugin.logout()");
        if (this._gauth){
            this._gauth.signOut();
            document.body.removeChild(this._script);
            this._script.removeEventListener("load", this._googleLoginEventListener);
            console.log("WebAuthPlugin.logout() Signed Out!");
        }
    }

    onUserUpdated: ISimpleEvent<User> = this._userUpdated.asEvent();

    addGoogleLoginEventListener(_: Event) {
        gapi.load('auth2', {
            callback: this._gapiLoad,
            onerror: this._gapiError,
            timeout: 5000, // 5 seconds.
            ontimeout: this._gapiTimeout
        });
    }

    onGapiAuthError(){
        alert('gapi.client failed to load!');
    }

    onGapiAuthTimeout(){
        alert('gapi.client could not load in a timely manner!');
    }

    onGapiAuthLoaded() {
        console.log("web.auth.plugin.initGapiClient: ");
        this._gauth = gapi.auth2.getAuthInstance();
        this._gauth.isSignedIn.listen(() => this.authIsSignedIn())
    }

    authIsSignedIn() {
        var googleUser = this._gauth.currentUser.get();
        var profile = googleUser.getBasicProfile();

        this._userUpdated.dispatch({
            firstName: profile.getGivenName(),
            lastName: profile.getFamilyName(),
            name: profile.getName(),
            imageUrl: profile.getImageUrl()
        });

          console.log("Sign In js ID: " + profile.getId()); // Don't send this directly to your server!
          console.log('Full Name: ' + profile.getName());
          console.log('Given Name: ' + profile.getGivenName());
          console.log('Family Name: ' + profile.getFamilyName());
          console.log("Image URL: " + profile.getImageUrl());
          console.log("Email: " + profile.getEmail());

        // The ID token you need to pass to your backend:
        // var id_token = googleUser.getAuthResponse().id_token;
        // console.log("ID Token: " + id_token);
    }
}