using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManagerScript : MonoBehaviour
{
    public List<GameObject> Pages;
    public static PageManagerScript instance;

    //Singleton pattern will be improved
    private void Awake() {
        instance = this;
    }

    public void swapToPage(int index) {
        for(int i = 0; i < Pages.ToArray().Length; i++) {
            if (i == index) {
                Pages[i].SetActive(true);
            } else {
                Pages[i].SetActive(false);
            }

        }


    }


    // Start is called before the first frame update
    void Start()
    {
        swapToPage(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
