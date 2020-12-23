using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTagDisplayScript : MonoBehaviour
{

    public Default_MapPageScript MapPageScript; //Reference to UI script so OnMouseDown can call function on it.

    public Image Pointer;
    //These are set in the editor as Cat-1 and Cat-2... and added to this list.
    public List<GameObject> CatVisuals;
    public float lat;
    public float lon;
    public int count;

    
    public MapTagDisplayScript(string input) {
        ReadAndDisplayNeed(input);
    }

    private void Update() {
        this.transform.LookAt(Camera.main.transform.position);
    }

    public bool ReadAndDisplayNeed(string input) {
        NeedData toRead = DataRoot.ReadFromJson(input) as NeedData;
        if (toRead.isInit()) {
            //Placeholder until data structure is more defined
            UpdateDisplay(toRead);
            return true;
        } else {
            Debug.LogError("MapTag Display Error: Data is uninitalized.");
            return false;
        }

    }


    public void UpdateDisplay(NeedData Need) {
        

       

    }

    public void DestroyMe() {
        Debug.Log("Destroy on MapTag called!");
        Destroy(this.gameObject);
    }

    public void OnMouseDown() {
        MapPageScript.ProvideNeedOnTag(this.gameObject);
    }


    private void OnBecameInvisible() {
        this.gameObject.SetActive(false);
    }

    private void OnBecameVisible() {
        this.gameObject.SetActive(true);
    }

}
