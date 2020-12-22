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
    private static Dictionary<NeedData, string> ReferenceKeptKeys;

    //Move all send data to server stuff to this class. (Default_MapPageScript should not have direct connection to server, only through this class.)
    string uniqueid;
    string key;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ReadFromCluster = new List<NeedData>();
        ReferenceKeptKeys = new Dictionary<NeedData, string>();

        if (_instance != null) {
            Debug.LogError("Can not have 2 instances of DataManagerScript");        }

        if (_instance != this) {
            _instance = this;
        }

        
    }


    public static DataManagerScript GetInstance() {
        return _instance;
    }


    //public async void ReadMapTagsFromServer() {
    //    DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    //    DataSnapshot snapshots = await reference.Child("Ihtiyaçlar").GetValueAsync();
    //    //Debug.Log("Snapshot child count: " + snapshots.ChildrenCount);
    //    foreach(DataSnapshot User in snapshots.Children) {
    //        //Debug.Log("Raw Json of child:" + User.GetRawJsonValue());

    //        foreach(DataSnapshot ND in User.Children) {
    //            //Debug.Log("Raw Need Data:" + ND.GetRawJsonValue());
    //            NeedData nd = JsonUtility.FromJson<NeedData>(ND.GetRawJsonValue());
    //            Debug.Log(nd.ToStringCustom());

    //        }


    //    }


    //}

    //Also saves references to their paths in ReferenceKeptKeys
    public async Task ReadMapTagsClustered(string cls) {
        ReadFromCluster.Clear();
        ReferenceKeptKeys.Clear();

        string[] clusters = cls.Split(',');
        int.TryParse(clusters[0],out int cx);
        int.TryParse(clusters[1], out int cy);

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        DataSnapshot snapshots = await reference.Child("Ihtiyaçlar").GetValueAsync();
       // Debug.Log("Snapshot child count: " + snapshots.ChildrenCount);
        foreach (DataSnapshot User in snapshots.Children) {
            //Debug.Log("Raw Json of child:" + User.GetRawJsonValue());

            foreach (DataSnapshot ND in User.Children) {
                //Debug.Log("Raw Need Data:" + ND.GetRawJsonValue());
                NeedData nd = JsonUtility.FromJson<NeedData>(ND.GetRawJsonValue());
                if(nd.ClusterX == cx && nd.ClusterY == cy) {

                    ReadFromCluster.Add(nd);
                    ReferenceKeptKeys.Add(nd, ND.Reference.ToString().Substring(ND.Reference.Root.ToString().Length - 1));
                }

                Debug.Log(nd.ToStringCustom());

            }

            Debug.Log("Read " + ReadFromCluster.Count + " maptags from cluster " + cls);

        }


    }

    public List<NeedData> GetLocalLocations() {
        if (ReadFromCluster.Count == 0) {
            Debug.Log("Clustered Locations read empty.");
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

            PatiLocationScript.GetInstance().PullUpdatedTags();


            return;
        }
    }


    public async void UpdateNeedWithProvide(GameObject submenu)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Provide").Push().Key;
        NeedData Need = PatiLocationScript.GetInstance().ClosestNeedData();
        string ReferenceKeyNeed = ReferenceKeptKeys[Need];
        Debug.Log("Firebase path for closest Need Data is: "+ReferenceKeyNeed);

        ProvideSubMenuScript pm = submenu.GetComponent<ProvideSubMenuScript>();
        if (pm)
        { //pm is null if submenu doesn't contain the script (ProvideSubMenuScript)
            ProvideData pd = pm.ReadData();
            string dat = pd.GetAsJson();
            Debug.Log("ProvideData to be calculated: " + dat);
            NeedData UpdatedData = ConversionBase.EditNeedData(Need, pd);
            Debug.Log("Data Updated to: " + UpdatedData.ToStringCustom());

            if (UpdatedData.fulfilled) {
                

                await reference.Child(ReferenceKeyNeed).RemoveValueAsync().ContinueWith(task => {
                    if(task.IsCompleted)
                    Debug.Log("Need FULFILLED! Yay! Removed it from server");
                }
                    );


            } else {

                await reference.Child(ReferenceKeyNeed).SetRawJsonValueAsync(UpdatedData.GetAsJson()).ContinueWith(task => {
                    if (task.IsCompleted)
                        Debug.Log("Need UPDATED! Check for update on server.");
                }
                    );

            }

            


            //Saves provide data on server.
            await reference.Child("Provide").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

        }

        ReadUserScoresFromFirebase();
        CalculateScores(submenu);
        PatiLocationScript.GetInstance().PullUpdatedTags();
        return;
    }

   

    public async void SendAnnoData(AnnoData And) {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        uniqueid = Authentication.uid;
        key = reference.Child("Duyuru").Push().Key;


        string dat = And.GetAsJson();
        Debug.Log("Anno to be calculated: " + dat);

        await reference.Child("Duyuru").Child(uniqueid).Child(key).SetRawJsonValueAsync(dat);

        //UpdateAnnouncement("-MOv86QV5IfH6VrSNx1Y");
        AnnouncementsFromFirebase();
        return;


    }


    //Note from jfox, 22 DEC
    //From this points downwards are the functions written by the student mainly responsible for the database functionality.
    //I'm sorry to write this but the below function is so unforgivabely bad that I have to leave it untouched.
    //I need to fix this class and make the database work, instead of working on the Social aspect of the app.
    //I can not in good conscience defend anything about this CalculateScores function. Half of it is irrelevant 
    //copy-paste and the rest is an unusable if chain that physically hurts me when I look at it. Also, Please tell my why this function has a submenu parameter?
    //Again, I am sorry to say this but I would expect WAY more from a near-graduate student who has put actual effort into this project.
    //Yet, this is borderline to doing nothing and even hurting the development process,
    //especially when demo is in 3 days.

    //It calculates score and send these scores to firebase
    public async void CalculateScores(GameObject submenu)
    {

        ProvideSubMenuScript pm = submenu.GetComponent<ProvideSubMenuScript>();
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        if (pm)
        {
            ProvideData nd = pm.ReadData();
            string dat = nd.GetAsJson();
            await FirebaseDatabase.DefaultInstance.GetReference("Score").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("ERROR");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    //Debug.Log("Score from firebase " + snapshot.Child(uniqueid).Value);
                    score = (int)((long)snapshot.Child(uniqueid).Value);
                }
            });

            //Scores for Water Amount
            if (nd.WaterAmount == "<100 ml")
            {
                score = score + 10;
            }
            if (nd.WaterAmount == "<300ml")
            {
                score = score + 30;
            }
            if (nd.WaterAmount == "<500 ml")
            {
                score = score + 40;
            }
            if (nd.WaterAmount == ">500 ml")
            {
                score = score + 50;
            }

            //Scores for Mama Amount
            if (nd.MamaAmount == "1")
            {
                score = score + 10;
            }
            if (nd.MamaAmount == "2")
            {
                score = score + 20;
            }
            if (nd.MamaAmount == "3")
            {
                score = score + 30;
            }
            if (nd.MamaAmount == "+4")
            {
                score = score + 40;
            }

            await reference.Child("Score").Child(uniqueid).SetValueAsync(score);

            Debug.Log("Score " + score);
        }

    }

    public async void AllNeedsFromFirebase()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot snapshots = await reference.Child("Ihtiyaçlar").GetValueAsync();


        foreach (DataSnapshot User in snapshots.Children)
        {
            Debug.Log("AllNeedsFromFirebase : " + User.GetRawJsonValue());

        }


    }

    //It reads all scores from firebase
    public async void ReadUserScoresFromFirebase() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot s = await reference.Child("Score").GetValueAsync();

        foreach (DataSnapshot User in s.Children) {
            // Debug.Log("User Scores :" + User.GetRawJsonValue());

        }


    }

    public async void AnnouncementsFromFirebase()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot snapshots = await reference.Child("Duyuru").GetValueAsync();


        foreach (DataSnapshot User in snapshots.Children)
        {
            Debug.Log("Duyurular : " + User.GetRawJsonValue());

        }


    }

    //It removes announcement.
    public async void UpdateAnnouncement(string s)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DataSnapshot snapshots = await reference.Child("Duyuru").GetValueAsync();


        foreach (DataSnapshot User in snapshots.Children)
        {
            Debug.Log("Duyurular : " + User.GetRawJsonValue());
            foreach (DataSnapshot duyuru in User.Children)
            {

                await FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").Child(User.Key).Child(s).RemoveValueAsync().ContinueWith(task =>
                {
                    Debug.Log("Delete");

                });

            }

        }


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
