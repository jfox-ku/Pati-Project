using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPageScript : UIPageScript {
    //Private to public static to access email and password in user script.
    public static string email;
    public static string password;
    public Authentication auth;



    public override void ReadInputFields() {
        if (inputFields.Length == 0) {
            Debug.LogError("Input Fields are empty");
        }
        email = inputFields[0].text;
        password = inputFields[1].text;
        Connect();

    }
    

    public void Connect() {

        Debug.Log("\n**email: " + email + "\n**Password: " + password);
        //Establish connection to database here
        //Below is the user to default to, purely to avoid typing it out every time. The user will instead have the option to autofill their last succesfull login.
        if(email == "" || password == "") {
            email = "kedicik@miyav.com";
            password = "banamama";
        }
        auth.setUserData(email, password);
        auth.LoginButton();

    }

}
