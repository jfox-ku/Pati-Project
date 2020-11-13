using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] //Serializable means this can be saved as plain text
public class DataRoot 
{
    private bool Initialized = false;

    public string AnimalType;
    public int AnimalCount;
    //This location variable might be changed to something other than a string,
    //but that object will also be serialazible so won't cause issues
    public string Location;
    
    public DataRoot(string Type, int Count, string Loc) {
        this.AnimalType = Type;
        this.AnimalCount = Count;
        this.Location = Loc;
        Initialized = true;

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
