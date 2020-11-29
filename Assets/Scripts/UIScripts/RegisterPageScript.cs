﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPageScript : UIPageScript
{
    public static string email;
    public static string password;
    public static string passwordCheck;
    public Authentication auth;

    public override void ReadInputFields() {
        if (inputFields.Length == 0) {
            Debug.LogError("Input Fields are empty");
        }
        email = inputFields[0].text;
        password = inputFields[1].text;
        passwordCheck = inputFields[2].text;
        if(password==passwordCheck)
        Register();
    }

    public void Register() {
        //Send register data to server here.
        //You can call a function on the auth class.
        //(Check the connect function in LoginPageScript)
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}