using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] //Serializable means this can be saved as plain text
public class DataRoot 
{
    //Could have used Enums here but a fixed string representation will be easier
    //to send to the server and be understood 
    public const string NEED_TYPE = "need";
    public const string PROVIDE_TYPE = "provide";
    public const string ANNO_TYPE = "anno";

    protected bool Initialized = false;
    protected string DataType;
    public string CreationDate;
    
    //Lat and Lon of the user at the time the user creates the data. These should not change at all after their initialization.
    public double Lat;
    public double Lon;
    public float ClusterX;
    public float ClusterY;

    //This location variable might be changed to something other than a string,
    //but that object will also be serialazible so won't cause issues
    public DataRoot(string DataType) {
        this.DataType = DataType;
        Initialized = true;
        CreationDate = DateTime.Now.ToShortDateString(); //current date
        Lat = PatiLocationScript.GetInstance().UserLoc.x;
        Lon = PatiLocationScript.GetInstance().UserLoc.y;
        //**

        string cluster = PatiLocationScript.FindCluster(Lat, Lon);
        Vector2 clst = PatiLocationScript.ExtractCluster(cluster);
        ClusterX = clst.x;
        ClusterY = clst.y;
    
        //**

    }

    public string GetLatLonString() {
        return Lat +","+Lon;
    }



    //Returns string representation of this object formatted as a json object
    public string GetAsJson() {
        return JsonUtility.ToJson(this);
    }

    //Returning an entirely new DataRoot object seems unnecessary,
    //but this will ensure scalability for now. No need to set each variable individually.
    public static DataRoot ReadFromJson(string input) {
        return JsonUtility.FromJson<DataRoot>(input);
    }

    public bool isInit() {
        return Initialized;
    }

}
