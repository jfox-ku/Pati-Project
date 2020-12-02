using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPanelScript : MonoBehaviour
{
    public GameObject Panel;

    public void TogglePanel() {
        if (Panel.activeInHierarchy) {
            Panel.SetActive(false);
        } else {
            Panel.SetActive(true);
        }
    }

}
