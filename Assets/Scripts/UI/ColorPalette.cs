using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    //this script is on an empty in the do not destroys
    public static ColorPalette singleton;
    //default color
    public Color defaultColor;
    //color of text when gaining a buff (Green)
    public Color buff;
    //color of text when gaining a debuff(red)
    public Color debuff;
    
    public Color allyHealthBar;
    public Color enemyHealthBar;

    public Color commonRarity;
    public Color rareRarity;
    public Color epicRarity;
    public Color legendaryRarity;

    //public static Color neutralHealthBar;


    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        //Debug.Log(allyHealthBar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
