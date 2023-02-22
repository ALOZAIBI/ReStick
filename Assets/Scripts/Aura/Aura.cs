using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Aura : MonoBehaviour
{
    public Character caster;

    public float amt;
    //wether this targets enemy otr ally or both 
    public bool enemy;
    public bool ally;

}
