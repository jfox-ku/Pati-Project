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

    protected bool Initialized = false;
    protected string DataType;
   
    //This location variable might be changed to something other than a string,
    //but that object will also be serialazible so won't cause issues
    
    
    public DataRoot(string DataType) {
        this.DataType = DataType;
        Initialized = true;

    }



    //Returns string representation of this object formatted as a json object
    //Note: Not sure how this will work with subclasses (NeedData/ProvideData)
    //They might need their own GetAsJson method.
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
