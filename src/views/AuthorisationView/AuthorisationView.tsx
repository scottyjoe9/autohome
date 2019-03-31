// import * as Expo from 'expo'
// import Markdown from 'react-native-markdown-renderer';
import * as React from 'react';
import {
  Button,
  StyleSheet,
  Text,
  View,
} from 'react-native';
import * as firebase from 'firebase';
// import { Load } from './Infrastructure/Load';
// import { AuthorisationView } from './AuthorisationView';
import * as BabelCore from 'babel-core';

var result = BabelCore.transform('<Text></Text>').code;

if (!result)
{
  result = "";
}
var component = eval(result);

// Load.Layout("AuthorisationView", require("../assets/Views/AuthorisationView.xml"));

export default class App extends React.Component {
  state = {
    username: undefined,
  };

  _loginWithGoogleAndFirebase = async () => {
    //   var config: any = {clientId: GoogleApiKey, scopes: ['profile', 'email'], behavior: "web"};
    // var result = await Google.logInAsync(config);

    // console.log("RESULT: " + JSON.stringify(result));

    // if (result.type == "success")
    // {
        // const credential = firebase.auth.GoogleAuthProvider.credential(result.accessToken);

        firebase.auth().signInWithEmailAndPassword("scottyjoe9@gmail.com","s3lf-self!")
        .then((res => {
            console.log("RESULT 2: " + JSON.stringify(res));
        })).catch((error) => {
            // Handle Errors here.
            console.log("ERROR: " + JSON.stringify(error));
          });
    // }
  }

  render() {
    return (

    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    fontSize: 20,
    textAlign: 'center',
    marginTop: 40,
  },
});