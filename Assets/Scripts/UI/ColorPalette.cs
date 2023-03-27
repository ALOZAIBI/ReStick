using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    //this script is on an empty in the do not destroys

    //default color
    public static Color defaultColor;
    //color of text when gaining a buff (Green)
    public static Color buff;
    //color of text when gaining a debuff(red)
    public static Color debuff;

    public static Color allyHealthBar;
    public static Color enemyHealthBar;

    //public static Color neutralHealthBar;


    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor);
        ColorUtility.TryParseHtmlString("#17B72D", out buff);
        ColorUtility.TryParseHtmlString("#D21F1F", out debuff);
        ColorUtility.TryParseHtmlString("#FF0000", out enemyHealthBar);
        ColorUtility.TryParseHtmlString("#6CD447", out allyHealthBar);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
