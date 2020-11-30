using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnnouncementDisplayScript : MonoBehaviour
{
    public GameObject details;

    public TextMeshProUGUI detailsText;
    public TextMeshProUGUI displayText;
    

    public void ToggleDetails()
    {
        if (details.activeInHierarchy)
        {
            details.SetActive(false);
        }
        else
        {
            details.SetActive(true);
        }
    }

    public void UpdateInformation(string display, string details)
    {
        detailsText.text = details;
        displayText.text = display;
    }
}
