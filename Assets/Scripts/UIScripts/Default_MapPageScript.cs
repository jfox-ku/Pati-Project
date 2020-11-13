using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Default_MapPageScript : UIPageScript
{
    //Could have used an array of buttons here for scalability
    //but this is better for clarity 
    public Button Mama_Button;
    public Button Need_Button;
    public Button Plus_Button;

    //Submenu objects that pop-up to ask for specific inputs
    //(AddNeed pop-up -> animal type and count)
    public GameObject AddNeedSubMenu;

    private bool isExpandedMenu = false;

    public override void ReadInputFields() {
        Debug.LogError("Nothing to read in Default Map Page");
    }


    public void ToggleNeedButtons() {
        
        if (isExpandedMenu) {
            //Close the other two buttons
            Mama_Button.gameObject.SetActive(false);
            Need_Button.gameObject.SetActive(false);
            isExpandedMenu = false;
        } else {
            //Open the other two buttons
            Mama_Button.gameObject.SetActive(true);
            Need_Button.gameObject.SetActive(true);
            isExpandedMenu = true;
        }



    }

    public void AddNeed() {
        AddNeedSubMenu.SetActive(true);
    }

    public void ProvideNeed() {

    }

    public void QuitButtonOnSubmenu(GameObject submenu) {
        //maybe some call to reset the submenu to a default state?
        //**

        submenu.SetActive(false);
           

    }

    public bool SendDataToServer() {
        return false;
    }

}
