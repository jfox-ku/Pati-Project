using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnoData : DataRoot
{
    public string Header;
    public string Details;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AnnoData(string Header, string Details) : base(ANNO_TYPE) {
        //CreatorUser = Authentication.uid;
        this.Header = Header;
        this.Details = Details;

    }
}
