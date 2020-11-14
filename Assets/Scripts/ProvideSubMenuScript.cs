using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProvideSubMenuScript : SubMenuScript
{
    //Variables for AddNeed
    public TMP_Dropdown WaterAmount;
    public TMP_Dropdown MamaAmount;



    public ProvideData ReadData() {
        string water = WaterAmount.options[WaterAmount.value].text;
        string mama = MamaAmount.options[MamaAmount.value].text;

        return new ProvideData(water, mama);

    }


}
