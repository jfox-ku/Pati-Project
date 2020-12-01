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

    
    public bool isVerifyEmailSend = false;
    
    
    
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


   public void RegisterButton() {
        Debug.Log("RegisterPress");
        //Call the login coroutine passing the email and password
        StartCoroutine(Register(email, password));
    
    }


   private IEnumerator Register(string _email, string _password) {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
        //Wait until the Register task completes
        
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
       
       Firebase.Auth.FirebaseUser newUser = RegisterTask.Result;
       SendEmailForVerification();
       if(isVerifyEmailSend == true)
       Debug.LogFormat("Firebase user created successfully: {0} ({1})",
       newUser.DisplayName, newUser.UserId);
       PageManagerScript.instance.swapToPage(1); 
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

//Sends an email for verification
      public void SendEmailForVerification()
    {
        if (auth.CurrentUser != null)
        {
            auth.CurrentUser.SendEmailVerificationAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Task was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Task encountered an error: " + task.Exception);
                    return;
                }
                auth.SignOut();
                isVerifyEmailSend = true;
                Debug.Log("Email send successfully.");
            });

        }

    }


    //Add IEnum for Register functionality.

}
