using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEditor;


public class UserLocationGPS : MonoBehaviour
{

    public const float LocUpdateTime = 1f;
    
// Start is called before the first frame update
public void StartLocation()
    {

#if UNITY_ANDROID
                  if (!Permission.HasUserAuthorizedPermission (Permission.FineLocation))
                  {
                           Permission.RequestUserPermission (Permission.FineLocation);
                  }
#elif UNITY_IOS
                  UnityEditor.PlayerSettings.iOS.locationUsageDescription = "Details to use location";
                  
#endif
        StartCoroutine(StartLocationService());

    }
    private IEnumerator StartLocationService()
    {
        yield return new WaitForSeconds(2);
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled location. Using placeholder location instead.");
            //PatiLocationScript.GetInstance().UserLocationStart(41.046139, 28.985556);
           
        }
        Input.location.Start();
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(2f);
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }

        int maxWait = 10;
        while(Input.location.status != LocationServiceStatus.Running) {
            Debug.Log("Waiting for Running status: "+maxWait);
            maxWait--;
            if (maxWait <= 0) {
                break;
            }

            yield return new WaitForSeconds(1f);
        }

        if(Input.location.status == LocationServiceStatus.Running) {
            double lat = Input.location.lastData.latitude;
            double lon = Input.location.lastData.longitude;
            Debug.Log("Latitude : " + lat);
            Debug.Log("Longitude : " + lon);

            if (lat == 0 || lon == 0) {
                lat = 41.046139;
                lon = 28.985556;
                Debug.LogError("Using default gps values. Use Unity Remote.");
            }

            StartCoroutine(UpdateLocation());
            PatiLocationScript.GetInstance().UserLocationStart(lat, lon);

        } else {
            Debug.LogError("Location service is not running. ");
        }
        

        

        

       // Debug.Log("Altitude : " + Input.location.lastData.altitude);
    }

    private IEnumerator UpdateLocation() {
        PatiLocationScript PatiLoc = PatiLocationScript.GetInstance();
        WaitForSeconds updateTime = new WaitForSeconds(LocUpdateTime);
        while (Input.location.status == LocationServiceStatus.Running) {
            double lat = Input.location.lastData.latitude;
            double lon = Input.location.lastData.longitude;

            PatiLoc.UpdateUserLocation(lat,lon);
            //waits for LocUpdateTime seconds
            yield return updateTime;
        }
        

    }


}
