using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using System;
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


    


    //Submenu objects that pop-up to ask for specific inputs
    //(AddNeed pop-up -> animal type and count)
    public GameObject AddNeedSubMenu;
    public GameObject ProvideNeedSubMenu;
    public GameObject LeftSubMenu;
    public GameObject UserSubMenu;
    public GameObject InfoSubMenu;
    public GameObject MapPanel;

    //Announcments stuff (Duyurular)
    public List<GameObject> announcementsList;
    public GameObject announcementPrefab;
    public Transform startTransform;

    private bool isExpandedNeedMenu = false;

    public void Start()
    {
        announcementsList = new List<GameObject>();
    }


    //Not hooked up to a submenu. So a few placeholder strings are used.
    public void AddAnnouncement()
    {
        var obj = Instantiate(announcementPrefab);
        obj.transform.SetParent(LeftSubMenu.transform);
        obj.transform.localScale = new Vector3(0.7f,0.7f,1f);
        announcementsList.Add(obj);
        AnnouncementDisplayScript objectScript = obj.GetComponent<AnnouncementDisplayScript>();
        switch (UnityEngine.Random.Range(0, 3)) {
            case 0: objectScript.UpdateInformation("Acil ilaç ihtiyacı.", "Ataşehir'de. İletişim: 39450349");
                break;
            case 1:
                objectScript.UpdateInformation("Yavru sahiplenme.", "Eminönün'de. İletişim: mymail@gmail.com");
                break;
            case 2:
                objectScript.UpdateInformation("Köpek topluluğu. Gece dikkat edin.", "Sarıyer'de.");
                break;
            case 3:
                objectScript.UpdateInformation("Acil klube ihtiyacı.", "Maslak'ta. İletişim: 5312943021");
                break;
        }
        
        obj.transform.position = new Vector3(startTransform.position.x, startTransform.position.y + (announcementsList.Count * -130f), startTransform.position.z);

        AnnoData AnData = objectScript.ReadAsAnnoData();
        DataManagerScript.GetInstance().SendAnnoData(AnData);

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

   
    public void ToggleUserMenu() {
        if (UserSubMenu.activeInHierarchy) {
            UserSubMenu.SetActive(false);
        } else {
            UserSubMenu.SetActive(true);
        }

    }
     public void ToggleInfoMenu()
        {
            if (InfoSubMenu.activeInHierarchy)
            {
                InfoSubMenu.SetActive(false);
            } else
            {
                InfoSubMenu.SetActive(true);
            }
        }

    public void TogglePanel()
    {
        if (MapPanel.activeInHierarchy)
        {
            MapPanel.SetActive(false);
        }
        else
        {
            MapPanel.SetActive(true);
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

    //Add SubMenu for Announcements, toggle submenu here 


    public void AddNeed() {
        AddNeedSubMenu.SetActive(true);
    
    }

    public void ProvideNeed() {
        ProvideNeedSubMenu.SetActive(true);
    }

    public void ProvideNeedOnTag(GameObject Tag) {
        ProvideNeedSubMenu.SetActive(true);
    }


    public void QuitButtonOnSubmenu(GameObject submenu) {
        submenu.SetActive(false);
    }
 
      
       

}

