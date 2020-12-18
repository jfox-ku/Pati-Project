using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

public class SpawnUserScript : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    [Geocode]
    string location;
    Vector2d userLoc;

    [SerializeField]
    float _spawnScale = 1f;

    [SerializeField]
    GameObject _markerPrefab;

    GameObject spawnedObject;
   

    public void SetUserLocation(string input)
    {
        location = input;
    }


    public void PlaceUserTag()
    {
        userLoc = Conversions.StringToLatLon(location);
        var instance = Instantiate(_markerPrefab);
        var UserTag = instance.GetComponentInChildren<UserDisplayScript>();
        UserTag.lat = double.Parse(userLoc.x + "");
        UserTag.lon = double.Parse(userLoc.y + "");

        instance.transform.localPosition = _map.GeoToWorldPosition(userLoc, true);
        instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        spawnedObject = instance;
    }



// Update is called once per frame
void Update()
    {
        if (spawnedObject == null) return;
        var location = userLoc;
        spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
        spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
    }
    
    

}
