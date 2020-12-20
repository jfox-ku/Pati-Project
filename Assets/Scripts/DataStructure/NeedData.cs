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

    public bool fulfilled = false;
    
  
    public NeedData(string AType, string ACount, string AMaturity) : base(NEED_TYPE) {
        //CreatorUser = Authentication.uid;
        AnimalType = AType;
        AnimalCount = ACount;
        AnimalMaturity = AMaturity;

    }

   
    public string GetLocationString() {
        return PatiLocationScript.PutLatLon(Lat, Lon);
    }


    public string ToStringCustom() {
        string ret = "Need Data created on " + CreationDate + "in cluster: " + ClusterX + ", " + ClusterY;
        ret += "\nType: "+AnimalType+"\nCount: "+AnimalCount;
        return ret;
    }




}
