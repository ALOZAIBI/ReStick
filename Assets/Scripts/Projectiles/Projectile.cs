using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    //these values are set by the calling character
    public Character target;
    //the instantiating character
    public Character shooter;
    public float speed;
    public float lifetime;
    public float dmg;
    public float LS;

    //handles the trajectory of the projectile
    public abstract void trajectory();


}
