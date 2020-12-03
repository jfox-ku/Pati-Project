using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubMenuScript : MonoBehaviour
{
    public const string NEED_MENU = "need_menu";
    public const string PROVIDE_MENU = "provide_menu";

    public string MenuType;

    public string getType() {
        return MenuType;
    }
}
