using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPageScript : UIPageScript {
    private string email;
    private string password;




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

    }

}
