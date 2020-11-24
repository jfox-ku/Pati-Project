using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PatiLocationScript : MonoBehaviour
{
    public SpawnOnMap MapSpwn; //This holds reference to script on map object

    public string[] ListOfLocations;

    // Start is called before the first frame update
    void Start()
    {
        ListOfLocations = new string[] {"41.193139,29.049372","41.194786,29.052225"};
        
        MapSpwn.SetLocations(ListOfLocations);
        MapSpwn.PlaceMapTags();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
