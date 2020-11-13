using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTagDisplayScript : MonoBehaviour
{
    public Image Pointer;
    //These are set in the editor as Cat-1 and Cat-2... and added to this list.
    public List<GameObject> CatVisuals;
    
    public MapTagDisplayScript(string input) {
        ReadAndDisplayNeed(input);
    }


    public bool ReadAndDisplayNeed(string input) {
        DataRoot toRead = DataRoot.ReadFromJson(input);
        if (toRead.isInit()) {

            UpdateDisplay(toRead.AnimalCount,toRead.Location);
            return true;
        } else {
            Debug.LogError("MapTag Display Error: Data is uninitalized.");
            return false;
        }

    }


    public void UpdateDisplay(int Count, string Loc) {
        foreach(GameObject Visual in CatVisuals) {
            Visual.SetActive(false);
        }

        CatVisuals[Count-1].SetActive(true);

    }

}
