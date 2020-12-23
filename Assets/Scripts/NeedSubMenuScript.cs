using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedSubMenuScript : SubMenuScript
{
  
    //Variables for AddNeed
    public TMP_Dropdown AnimalType;
    public TMP_Dropdown AnimalCount;
    public TMP_Dropdown AnimalMaturity;


    public NeedData ReadData() {
        string type = AnimalType.options[AnimalType.value].text;
        string count = AnimalCount.options[AnimalCount.value].text;
        string maturity = AnimalMaturity.options[AnimalMaturity.value].text;

        return new NeedData(type,count,maturity);

    }

   

   






}
