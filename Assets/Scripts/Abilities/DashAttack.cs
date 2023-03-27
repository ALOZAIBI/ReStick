using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{
    public float dashSpeed;
    //how much cd will be at once it's reset
    private float cdReset=0.2f;

    //Maybe add this in the future it is taken frmo the initial stick project check my gitlab
    //public float resetPossibility = 0.7f;   //target has 0.7 seconds to die after dash for the dash to reset
    private void Start() {
        updateDescription();
    }

    private void Update() {
        cooldown();
    }
    public override void doAbility() {
        if (available) {
            //selects target
            character.selectTarget(targetStrategy);
            //dashes to target
            character.transform.position = Vector2.MoveTowards(character.transform.position, character.target.transform.position, dashSpeed*Time.fixedDeltaTime);
            //once in range deal damage and start CD if kills target reset CD 0.5f is the range in this case
            if (Vector2.Distance(character.transform.position, character.target.transform.position) < 0.5f) {
                //deal damage
                character.target.HP -= amt;
                //apply life steal
                character.HP += amt * character.LS;
                if (character.target.HP < 0) {
                    character.totalKills++;
                    character.killsLastFrame++;
                    //reset cd
                    startCooldown();
                    CD = cdReset;
                    //Do nothing since CD hasn't started in this case
                }
                else {
                    //start cd
                    startCooldown();
                }
            }
            
        }
    }

    public override void updateDescription() {
        description = "Dash towards target dealing " + amt + "DMG reset CD if this kills";
    }


}
