using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeedSubMenuScript : SubMenuScript
{
  
    //Variables for AddNeed
    public TMP_Dropdown AnimalType;
    public TMP_Dropdown AnimalCount;

 

    public NeedData ReadData() {
        string type = AnimalType.options[AnimalType.value].text;
        string count = AnimalCount.options[AnimalCount.value].text;

        return new NeedData(type,count);

    }






}
