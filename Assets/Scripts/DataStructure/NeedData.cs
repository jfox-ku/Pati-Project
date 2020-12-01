using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeedData : DataRoot {

    //Unique id of user who created this NeedData
    //public string CreatorUser;

    public string AnimalType;
    public string AnimalCount;
    public string AnimalMaturity;
    public string CreationDate = DateTime.Now.ToShortDateString(); //current date
   
    //Lat and Lon of the user at the time the user creates the data. These should not change at all after their initialization.
    public float Lat;
    public float Lon;


    public NeedData(string AType, string ACount) : base(NEED_TYPE) {
        //CreatorUser = Authentication.uid;
        AnimalType = AType;
        AnimalCount = ACount;

    }

   
    public string GetLocationString() {
        return PatiLocationScript.PutLatLon(Lat, Lon);
    }



}
