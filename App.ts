import AuthorisationViewModel from './src/views/AuthorisationView/AuthorisationView';
export default AuthorisationViewModel;
import * as firebase from 'firebase';

// Initialize Firebase
const firebaseConfig = {
  apiKey: "AIzaSyC8x5uUebOaDx-9A7Vr1HMaxEB2rBRCppQ",
  authDomain: "autohome-e745f.firebaseapp.com",
  databaseURL: "https://autohome-e745f.firebaseio.com",
  storageBucket: "autohome-e745f.appspot.com"
};

firebase.initializeApp(firebaseConfig);
