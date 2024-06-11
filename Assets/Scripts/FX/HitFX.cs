using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For playing FX animations that have multiple parts
public class HitFX : MonoBehaviour
{
    public KeepOnTarget keepOnTarget;

    [SerializeField] Animator animator;

    //If this reaches 0 endAnimation
    public float durationRemaining = 0.07f;
    //Plays animation when animation event happens destroy the object
    public void DestroyFX() {
        Destroy(gameObject);
    }

    public void playEndAnimation() {
        animator.SetTrigger("End");
    }

    public void refreshDuration() {
        durationRemaining = 0.15f;
    }
    private void FixedUpdate() {
        if (keepOnTarget != null) {
            durationRemaining -= Time.fixedDeltaTime;
            if (durationRemaining <= 0) {
                playEndAnimation();
            }
        }

    }
}
