using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{//This class is used to keep users in the database with other datas.
	//User mail and password
    public string userMail;
    public string userPassword;
    public string localId;

    public User()
    {
    	//It takes email and password from LoginPageScript.
        userMail = LoginPageScript.email;
        userPassword = LoginPageScript.password;
        
    }
}