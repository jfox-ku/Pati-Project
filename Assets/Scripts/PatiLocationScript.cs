using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PatiLocationScript : MonoBehaviour
{
    public SpawnOnMap MapSpwn; //This holds reference to script on map object
    private static PatiLocationScript _instance;


    public string[] ListOfLocations;

    //Testing
    public string[] ListOfLocationsTest;
    public Vector2 UserLoc;

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;

        //* Get user location here
        UserLoc = new Vector2(41.046639f, 28.984556f);

        //* Get user location cluster here. (Clusters are wider areas)
        string UserCluster = FindCluster(UserLoc.x, UserLoc.y);

        //* Get all NeedData within a cluster from the server, put them into the format seen below. Make use of the ExtractLatLon and PutLatLon functions defined below.
        ListOfLocations = new string[] {"41.193139,29.049372","41.194786,29.052225","52.357702,4.864808", "52.355959,4.863162"};
        ListOfLocationsTest = new string[] { "41.193139,29.049372", "41.194786,29.052225", "52.357702,4.864808", "52.355959,4.863162", "41.046139,28.985556" };

        //This bit is for testing the helper functions. Can be removed.
        foreach (string cord in ListOfLocations) {
            Vector2 vec = ExtractLatLon(cord);
            string clstr = FindCluster(vec.x, vec.y);
            if (CheckClusterValid(vec.x, vec.y,clstr)) {
                Debug.Log("Cluster No: "+clstr+"\nX: " + vec.x+"\nY: "+vec.y);
            }



        }

        MapSpwn.SetLocations(ListOfLocations);
        MapSpwn.PlaceMapTags();
    }


    //There is already a listener available with mapbox, so this function will subscribe to onLocationUpdated from LocationProvider
    //For now it is on a button
    public void UpdateLocation() {
        MapSpwn.UpdateAndPlaceTags(ListOfLocationsTest); //This shouldn't be test. Just to see if it updates.
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static string FindCluster(float lat,float lon) {
        int x = (int) Math.Round(lat / 5f);
        int y = (int) Math.Round(lat / 5f);
        return x + "," + y;

    }

    public static bool CheckClusterValid(float lat, float lon, string clust) {
        if (FindCluster(lat, lon).CompareTo(clust) == 0) {
            return true;
        } else {
            return false;
        }

    }


    public static Vector2 ExtractLatLon(string s) {
        float x = float.Parse( s.Substring(0,s.IndexOf(',')));
        float y = float.Parse(s.Substring(s.IndexOf(',')+1));


        return new Vector2(x, y);

    }

    public static Vector2 ExtractCluster(string s) {
        float x = float.Parse(s.Substring(0, s.IndexOf(',')));
        float y = float.Parse(s.Substring(s.IndexOf(',') + 1));


        return new Vector2(x, y);

    }

    public static string PutLatLon(float lat,float lon){
        return lat + "," + lon;

    }

    public static PatiLocationScript GetInstance() {   
        return _instance;
    }



}
