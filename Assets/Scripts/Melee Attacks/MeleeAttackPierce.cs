using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackPierce : MeleeAttack
{
    public bool maxSizeAttained;

    public float durationOfMaxSizeRemaining;

    public List<Character> charactersHit = new List<Character>();

    //angles the attack towards the target
    public void angle() {
        float angle = Mathf.Atan2(target.transform.position.y-emptyPivot.transform.position.y, target.transform.position.x-emptyPivot.transform.position.x)*Mathf.Rad2Deg;
        emptyPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    public void growAndShrink() {
        if (emptyPivot.transform.localScale.x <= range&&!maxSizeAttained) {
            //grow along the x axis over time with speed speed
            emptyPivot.transform.localScale = new Vector3(emptyPivot.transform.localScale.x + (speed * Time.fixedDeltaTime), emptyPivot.transform.localScale.y, emptyPivot.transform.localScale.z);
            durationOfMaxSizeRemaining = lifetime;
        }
        else {
            maxSizeAttained = true;
            if(durationOfMaxSizeRemaining > 0) {
                durationOfMaxSizeRemaining -= Time.fixedDeltaTime;
            }
            else {
                //shrink along the x axis over time with speed speed until reaching 0
                emptyPivot.transform.localScale = new Vector3(emptyPivot.transform.localScale.x - (speed * Time.fixedDeltaTime), emptyPivot.transform.localScale.y, emptyPivot.transform.localScale.z);

                if (emptyPivot.transform.localScale.x <= 0) {
                    Destroy(transform.parent.gameObject);
                }
            }
            
        }
        
    }
    //im doiong this instead of ontrigger enter because on trigger enter doesn't work on very first frame. read api
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "Character") {
            Character victim = collision.gameObject.GetComponent<Character>();
            if(victim.team != character.team&& !charactersHit.Contains(victim)) {
                character.damage(victim, DMG, LS);
                applyBuff(victim);
            }
            charactersHit.Add(victim);
        }
    }

    public override void attackHandler() {
        growAndShrink();
    }
    private void Start() {
        base.Start();
        angle();
    }

    private void FixedUpdate() {
        attackHandler();
    }
}
