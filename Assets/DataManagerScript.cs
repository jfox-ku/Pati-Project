using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerScript : MonoBehaviour
{
    private static DataManagerScript _instance;
    //Move all send data to server stuff to this class. (Default_MapPageScript should not have direct connection to server, only through this class.)


    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null) {
            Debug.LogError("Can not have 2 instances of DataManagerScript");        }

        if (_instance != this) {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    public static DataManagerScript GetInstance() {
        return _instance;
    }

    public void AddNewMapTag() {


    }

    public void ResolveExistingMapTag(GameObject Tag) {


    }
   
}
