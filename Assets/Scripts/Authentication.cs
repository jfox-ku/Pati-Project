using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;


public class Authentication : MonoBehaviour {
    private string email;
    private string password;
    public static string uid;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    
    
    public void setUserData(string e,string p) {
        email = e;
        password = p;

    }

    void Awake() {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            } else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Default Instance Set");
    }


    public void LoginButton() {
        Debug.Log("LoginPress");
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(email, password));
    
    }


    private IEnumerator Login(string _email, string _password) {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        Debug.Log("1");
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        Debug.Log("2");

        //User is now logged in
        //Now get the result
        User = LoginTask.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
        Debug.Log("4");

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            uid = user.UserId;
            //It prints user id.
            Debug.Log("user id " + uid);
        }
        PageManagerScript.instance.swapToPage(2); //Page 2 is Default_Map_page

    }


    //Add IEnum for Register functionality.

}
