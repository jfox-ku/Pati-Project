﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvideData : DataRoot
{
    public string WaterAmount;
    public string MamaAmount;

    public ProvideData(string water, string mama) : base(PROVIDE_TYPE) {
        WaterAmount = water;
        MamaAmount = mama;

    }

    
    

}
