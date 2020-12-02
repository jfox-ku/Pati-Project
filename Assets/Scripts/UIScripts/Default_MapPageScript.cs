﻿using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
 
   

public class Default_MapPageScript : UIPageScript {
    //Could have used an array of buttons here for scalability
    //but this is better for clarity 
    

    public Button Mama_Button;
    public Button Need_Button;
    public Button Plus_Button;
    
    string uniqueid;
    string key;
   


    //Submenu objects that pop-up to ask for specific inputs
    //(AddNeed pop-up -> animal type and count)
    public GameObject AddNeedSubMenu;
    public GameObject ProvideNeedSubMenu;
    public GameObject LeftSubMenu;

    //Announcments stuff (Duyurular)
    public List<GameObject> announcementsList;
    public GameObject announcementPrefab;
    public Transform startTransform;

    private bool isExpandedNeedMenu = false;

    public void Start()
    {
        announcementsList = new List<GameObject>();
    }

    public void AddAnnouncement()
    {
        var obj = Instantiate(announcementPrefab);
        obj.transform.SetParent(LeftSubMenu.transform);
        obj.transform.localScale = new Vector3(1f,1f,1f);
        announcementsList.Add(obj);
        AnnouncementDisplayScript objectScript = obj.GetComponent<AnnouncementDisplayScript>();
        objectScript.UpdateInformation("Acil ilaç ihtiyacı.", "Ataşehir'de. İletişim: 39450349");
        obj.transform.position = new Vector3(startTransform.position.x, startTransform.position.y + (announcementsList.Count * -50f), startTransform.position.z);
    }

    public override void ReadInputFields() {
        Debug.LogError("Nothing to read in Default Map Page");
    }

    public void ToggleLeftMenu() {
        if (LeftSubMenu.activeInHierarchy) {
            LeftSubMenu.SetActive(false);
        } else {
            LeftSubMenu.SetActive(true);
        }

    }

    public void ToggleNeedButtons() {

        if (isExpandedNeedMenu) {
            //Close the other two buttons
            Mama_Button.gameObject.SetActive(false);
            Need_Button.gameObject.SetActive(false);
            isExpandedNeedMenu = false;
        } else {
            //Open the other two buttons
            Mama_Button.gameObject.SetActive(true);
            Need_Button.gameObject.SetActive(true);
            isExpandedNeedMenu = true;
        }

    }

    public void AddNeed() {
        AddNeedSubMenu.SetActive(true);
    
    }

    public void ProvideNeed() {
        ProvideNeedSubMenu.SetActive(true);
    }

    public void ProvideNeedOnTag(GameObject Tag) {
        ProvideNeedSubMenu.SetActive(true);
    }


    //Doesn't work Currently
    public void QuitButtonOnSubmenu(GameObject submenu) {
        submenu.SetActive(false);
    }
  
    //Please don't mind the ugly structure of this function
    //We will improve the whole data system during the next work package (beyond week 5)
    //this is solely for testing
    public async void SendNeedDataToServer(GameObject submenu) {

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


            await FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").Child(key).Child(uniqueid).Child("AnimalType").GetValueAsync().ContinueWith(task => {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Retrieving data from database " + snapshot.GetRawJsonValue());

            });

            //it removes datas with given user ids.
            /* FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").Child(uniqueid).RemoveValueAsync().ContinueWith(task => {
               Debug.Log("Delete");

          });*/


            await FirebaseDatabase.DefaultInstance.GetReference("Ihtiyaçlar").OrderByChild("AnimalType").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.Log("Data not found");
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;

                    Debug.Log("query" + snapshot.GetRawJsonValue());
                }
            });
        
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

