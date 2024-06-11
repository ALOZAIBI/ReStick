using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFX : MonoBehaviour
{
    public KeepOnTarget keepOnTarget;

    public bool destroyOnLastFrame = true;

    //This will be called by last frame in animation
    public void DestroyFX() {
        if (destroyOnLastFrame)
            Destroy(gameObject);
    }


}
