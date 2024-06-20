using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For playing FX animations that have multiple parts
public class HitFX : MonoBehaviour
{
    public KeepOnTarget keepOnTarget;

    [SerializeField] Animator animator;

    //If true need to look at duration remaining
    public bool hasTimeLimit = true;
    //If this reaches 0 endAnimation
    public float durationRemaining = 0.07f;

    //The creating Object
    //For example in sheen it creates a hitFX, we want it so that when the animation is done, it calls the create projectile in sheen. THat is why we need to reference the creator
    public GameObject creator;

    private void Start() {
        setSize();
    }
    //Plays animation when animation event happens destroy the object
    public void DestroyFX() {
        //After destroying the object do what the creator wants to do
        //Checks if the creator is an item
        if (creator!=null && creator.GetComponent<Item>() != null) {
            Item item = creator.GetComponent<Item>();
            item.afterAnimation();
            Debug.Log("DestroyFX");
        }
        Destroy(gameObject);
    }

    public void playEndAnimation() {
        animator.SetTrigger("End");
    }

    public void refreshDuration() {
        durationRemaining = 0.15f;
    }
    //Sets size of the Fx to be equal to the character it's on
    public void setSize() {
        if(keepOnTarget != null)
            transform.localScale = keepOnTarget.target.transform.localScale;
    }
    private void FixedUpdate() {
        if (keepOnTarget != null) {
            if (hasTimeLimit) {
                durationRemaining -= Time.fixedDeltaTime;
                if (durationRemaining <= 0) {
                    playEndAnimation();
                }
            }
            //if the target is dead
            if (!keepOnTarget.character.alive) {
                playEndAnimation();
            }
        }

    }
}
