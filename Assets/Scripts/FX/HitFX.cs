using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    //Plays animation when animation event happens destroy the object
    public void DestroyFX() {
        Destroy(gameObject);
    }
}
