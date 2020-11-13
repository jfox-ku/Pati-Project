using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //TextMesh pro for better text visuals (provided by unity)

public abstract class UIPageScript : MonoBehaviour
{
    [SerializeField] //This line shows the array of input fields on the UnityEditor
    protected TMP_InputField[] inputFields;
    

    public abstract void ReadInputFields();





}
