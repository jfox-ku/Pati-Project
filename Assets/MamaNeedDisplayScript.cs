using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConversionBase;
using TMPro;

public class MamaNeedDisplayScript : MonoBehaviour
{
    public TMP_Text text;

    public void UpdateDisplay(NeedData n)
    {
        ConversionBase.AnimalNeed closestneed = new ConversionBase.AnimalNeed(n);
        string displaytext = closestneed.WFP.getAsString() + "\nEn yakındaki ihtiyaca olan uzaklık: " + PatiLocationScript.GetInstance().getClosestDist().ToString().Substring(0,3) + "m";
        text.text = displaytext;

    }
}
