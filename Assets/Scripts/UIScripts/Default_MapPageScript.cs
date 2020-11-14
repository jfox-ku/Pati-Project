using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

public class Default_MapPageScript : UIPageScript {
    //Could have used an array of buttons here for scalability
    //but this is better for clarity 
    public Button Mama_Button;
    public Button Need_Button;
    public Button Plus_Button;

    //Submenu objects that pop-up to ask for specific inputs
    //(AddNeed pop-up -> animal type and count)
    public GameObject AddNeedSubMenu;
    public GameObject ProvideNeedSubMenu;
    public GameObject LeftSubMenu;

    private bool isExpandedNeedMenu = false;

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

    public void QuitButtonOnSubmenu(GameObject submenu) {
        submenu.SetActive(false);
    }



    //Please don't mind the ugly structure of this function
    //We will improve the whole data system during the next work package (beyond week 5)
    //this is solely for testing
    public void SendNeedDataToServer(GameObject submenu) {

        NeedSubMenuScript sm = submenu.GetComponent<NeedSubMenuScript>();
        User user = new User();
        if (sm) { //sm is null if submenu doesn't contain the script (NeedSubMenuScript)
            NeedData nd = sm.ReadData();
            string dat = nd.GetAsJson();
            Debug.Log("NeedData to be sent: " + dat);
            // It keeps the type and number of animals with the user information in firebase database.
            RestClient.Put("https://pati-98498.firebaseio.com/ihtiyaçlar"+ dat + ".json", user);
            
            return;
        }

        ProvideSubMenuScript pm = submenu.GetComponent<ProvideSubMenuScript>();
        if (pm) {
            ProvideData pd = pm.ReadData();
            string dat = pd.GetAsJson();
            Debug.Log("ProvideData to be sent: " + dat);
            // It keeps the amount of water and the amount of food with the user information in firebase database.
            RestClient.Put("https://pati-98498.firebaseio.com/mamavesu"+ dat + ".json", user);
            
            


            
            //send dat to server here
            //**
            return;
        }



    }

}
