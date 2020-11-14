using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedData : DataRoot {
    public string AnimalType;
    public string AnimalCount;

    public NeedData(string AType, string ACount) : base(NEED_TYPE) {
        AnimalType = AType;
        AnimalCount = ACount;

    }

    //Will add this function if base class(DataRoot) call causes errors in testing
    //public string GetAsJson(){}


}
