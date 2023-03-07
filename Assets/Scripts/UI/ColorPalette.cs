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

    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor);

        ColorUtility.TryParseHtmlString("#17B72D", out buff);
        ColorUtility.TryParseHtmlString("#D21F1F", out debuff);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
