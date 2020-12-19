using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class DataManagerScript : MonoBehaviour
{
    private static DataManagerScript _instance;
    public const string NEED_DATA = "NEED_DATA";
    private List<NeedData> ReadFromCluster;

    //Move all send data to server stuff to this class. (Default_MapPageScript should not have direct connection to server, only through this class.)
    string uniqueid;
    string key;

    // Start is called before the first frame update
    void Start()
    {
        ReadFromCluster = new List<NeedData>();
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


    public async void ReadMapTagsFromServer() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot snapshots = await reference.Child("Ihtiyaçlar").GetValueAsync();
        Debug.Log("Snapshot child count: " + snapshots.ChildrenCount);
        foreach(DataSnapshot User in snapshots.Children) {
            Debug.Log("Raw Json of child:" + User.GetRawJsonValue());

            foreach(DataSnapshot ND in User.Children) {
                Debug.Log("Raw Need Data:" + ND.GetRawJsonValue());
                NeedData nd = JsonUtility.FromJson<NeedData>(ND.GetRawJsonValue());
                Debug.Log(nd.ToStringCustom());

            }


        }


    }

    public async Task ReadMapTagsClustered(string cls) {
        ReadFromCluster.Clear();
        string[] clusters = cls.Split(',');
        int.TryParse(clusters[0],out int cx);
        int.TryParse(clusters[1], out int cy);

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot snapshots = await reference.Child("Ihtiyaçlar").GetValueAsync();
        Debug.Log("Snapshot child count: " + snapshots.ChildrenCount);
        foreach (DataSnapshot User in snapshots.Children) {
            //Debug.Log("Raw Json of child:" + User.GetRawJsonValue());

            foreach (DataSnapshot ND in User.Children) {
                //Debug.Log("Raw Need Data:" + ND.GetRawJsonValue());
                NeedData nd = JsonUtility.FromJson<NeedData>(ND.GetRawJsonValue());
                if(nd.ClusterX == cx && nd.ClusterY == cy) {

                    ReadFromCluster.Add(nd);
                }

                Debug.Log(nd.ToStringCustom());

            }

            Debug.Log("Read " + ReadFromCluster.Count + " maptags from cluster " + cls);

        }


    }

    public List<NeedData> GetLocalLocations() {
        if (ReadFromCluster.Count == 0) {
            Debug.LogError("Clustered Locations read empty.");
        }


        return ReadFromCluster;
    }


    public async void SendNeedDataFromSubmenu(GameObject submenu) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Ihtiyaçlar").Push().Key;

        NeedSubMenuScript sm = submenu.GetComponent<NeedSubMenuScript>();


        if (sm) { //sm is null if submenu doesn't contain the script (NeedSubMenuScript)
            NeedData nd = sm.ReadData();
            string dat = nd.GetAsJson();
            Debug.Log("NeedData to be sent: " + dat);

            await reference.Child("Ihtiyaçlar").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);
            
            


            return;
        }
    }


    //Not used right now. Will update existing NeedData on the server depending on the 
    public async void UpdateNeedWithProvide(GameObject submenu) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Provide").Push().Key;

        ProvideSubMenuScript pm = submenu.GetComponent<ProvideSubMenuScript>();


        if (pm) { //sm is null if submenu doesn't contain the script (NeedSubMenuScript)
            ProvideData nd = pm.ReadData();
            string dat = nd.GetAsJson();
            Debug.Log("ProvideData to be calculated: " + dat);

            await reference.Child("Provide").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

            return;
        }

    }


    public async void SendAnnoData(AnnoData And) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Duyuru").Push().Key;


        string dat = And.GetAsJson();
        Debug.Log("Anno to be calculated: " + dat);

        await reference.Child("Duyuru").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

        return;


    }









    //This was for testing. Now still here for example purposes. Not actually called from any buttons or scripts.
    //Better version of this is up above, so no need to pay attention here.
    public async void SendNeedDataToServerOld(GameObject submenu) {

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Ihtiyaçlar").Push().Key;

        NeedSubMenuScript sm = submenu.GetComponent<NeedSubMenuScript>();


        if (sm) { //sm is null if submenu doesn't contain the script (NeedSubMenuScript)
            NeedData nd = sm.ReadData();
            string dat = nd.GetAsJson();
            Debug.Log("NeedData to be sent: " + dat);

            await reference.Child("Ihtiyaçlar").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

            /*FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").OrderByChild("AnimalType").EqualTo("Diğer").GetValueAsync().ContinueWith(task => {
             if (task.IsFaulted){
                   Debug.Log("Data not found"); }
                   else if (task.IsCompleted){
              DataSnapshot snapshot = task.Result;
              Debug.Log("Retrieving " + snapshot.GetRawJsonValue());}

         });*/


            //**** Retrieve data example here ***//

            //await FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").Child(key).Child(uniqueid).Child("AnimalType").GetValueAsync().ContinueWith(task => {
            //    DataSnapshot snapshot = task.Result;
            //    Debug.Log("Retrieving data from database " + snapshot.GetRawJsonValue());

            //});


            //it removes datas with given user ids.
            /* FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").Child(uniqueid).RemoveValueAsync().ContinueWith(task => {
               Debug.Log("Delete");

          });*/


            //await FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").OrderByChild("AnimalType").GetValueAsync().ContinueWith(task => {
            //    if (task.IsFaulted) {
            //        Debug.Log("Data not found");
            //    } else if (task.IsCompleted) {
            //        DataSnapshot snapshot = task.Result;

            //        Debug.Log("query" + snapshot.GetRawJsonValue());
            //    }
            //});

            //Query query = reference.OrderByChild("AnimalType").EqualTo("Diğer");
            return;
        }

        ProvideSubMenuScript pm = submenu.GetComponent<ProvideSubMenuScript>();
        if (pm) {
            ProvideData pd = pm.ReadData();
            string dat = pd.GetAsJson();
            Debug.Log("ProvideData to be sent: " + dat);

            // It keeps the amount of water and the amount of food with the user id.
            string key = reference.Child("MamaveSu").Push().Key;
            await reference.Child("MamaveSu").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

            return;
        }

    }


}
