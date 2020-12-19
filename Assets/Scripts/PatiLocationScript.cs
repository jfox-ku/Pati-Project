using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PatiLocationScript : MonoBehaviour
{
    public SpawnOnMap MapSpwn; //This holds reference to script on map object
    public SpawnUserScript reference;

    private static PatiLocationScript _instance;

    [SerializeField]
    AbstractMap _map;

    public List<string> ListOfLocations;

    //Testing
    public Vector2d UserLoc;
        

    // Start is called before the first frame update
    void Start()
    {
        _instance = this;
    }


    async public void UserLocationStart(double Lat, double Lon) {
        //* Get user location here
        UserLoc = new Vector2d(Lat, Lon);
        _map.SetCenterLatitudeLongitude(new Vector2d(Lat,Lon));
        //* Get user location cluster here. (Clusters are wider areas)
        string UserCluster = FindCluster(Lat, Lon);

        string[] TestLocations = { "41.193139,29.049372", "41.194786,29.052225", "52.357702,4.864808", "52.355959,4.863162" };
        ListOfLocations = new List<string>();

        ListOfLocations.AddRange(TestLocations);

        var DataManagerIns = DataManagerScript.GetInstance();
        //Might have a sync issue here. 
        await DataManagerIns.ReadMapTagsClustered(FindCluster(UserLoc.x,UserLoc.y));
        List<NeedData> NData= DataManagerIns.GetLocalLocations();
        if (NData == null || NData.Count == 0) {
            Debug.LogError("No Valid NeedData recieved at PatiLocationScript");
        }
        foreach(NeedData nd in NData) {
            ListOfLocations.Add(nd.GetLocationString());
        }

        MapSpwn.SetLocations(ListOfLocations);
        MapSpwn.PlaceMapTags();
        reference.PlaceUserTag();

    }

    
    public void UpdateLocation() {
        //MapSpwn.UpdateAndPlaceTags(ListOfLocationsTest); //This shouldn't be test. Just to see if it updates.
        DataManagerScript.GetInstance().ReadMapTagsFromServer();
    }

    public void UpdateUserLocation(double Lat, double Lon) {
        Debug.Log("User Location updated to: " + Lat + ", " + Lon);
        UserLoc = new Vector2d(Lat, Lon);
        reference.SetUserLocation(SafePutLatLon(Lat, Lon));
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static string FindCluster(double lat,double lon) {
        int x = (int) Math.Round(lat / 5f);
        int y = (int) Math.Round(lat / 5f);
        return x + "," + y;

    }

    public static bool CheckClusterValid(double lat, double lon, string clust) {
        if (FindCluster(lat, lon).CompareTo(clust) == 0) {
            return true;
        } else {
            return false;
        }

    }


    public static Vector2d ExtractLatLon(string s) {
        double x = double.Parse( s.Substring(0,s.IndexOf(',')));
        double y = double.Parse(s.Substring(s.IndexOf(',')+1));


        return new Vector2d(x, y);

    }

    public static Vector2 ExtractCluster(string s) {
        float x = float.Parse(s.Substring(0, s.IndexOf(',')));
        float y = float.Parse(s.Substring(s.IndexOf(',') + 1));


        return new Vector2(x, y);

    }

    public static string PutLatLon(double lat, double lon) {

        string ret = lat + "," + lon;
        Debug.Log("PutLatLon made: " + ret);
        return ret;

    }


    public static string SafePutLatLon(double lat,double lon){
        string slat = lat + "";
        slat = slat.Replace(",", ".");

        string slon = lon + "";
        slon = slon.Replace(",", ".");

        string ret = slat + "," + slon;
        Debug.Log("PutLatLon made: "+ret);
        return ret;

    }

    public static PatiLocationScript GetInstance() {   
        return _instance;
    }



}
