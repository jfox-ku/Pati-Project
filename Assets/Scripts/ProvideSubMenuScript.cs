using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProvideSubMenuScript : SubMenuScript
{
    //Variables for AddNeed
    public TMP_Dropdown WaterAmount;
    public TMP_Dropdown MamaAmount;

    public NeedData ClosestNeed;

    public MamaNeedDisplayScript MamaNeed;


    private void OnEnable() {
        ClosestNeed = FindObjectOfType<PatiLocationScript>().ClosestNeedData();
        MamaNeed.UpdateDisplay(ClosestNeed);
        Debug.Log(ClosestNeed.ToStringCustom());
    }

    public ProvideData ReadData() {
        string water = WaterAmount.options[WaterAmount.value].text;
        string mama = MamaAmount.options[MamaAmount.value].text;

        return new ProvideData(water, mama);

    }


}
