using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{
    public float dashSpeed;
    //how much cd will be at once it's reset
    private float cdReset=0.1f;

    //Maybe add this in the future it is taken frmo the initial stick project check my gitlab
    //public float resetPossibility = 0.7f;   //target has 0.7 seconds to die after dash for the dash to reset
    private void Start() {
        updateDescription();
    }

    private void FixedUpdate() {
        cooldown();
    }
    public override void doAbility() {
        if (available&& character.selectTarget(targetStrategy,rangeAbility)) {
            calculateAmt();
            //dashes to target
            character.agent.enabled = false;//to allow the target to dash through obstacles
            character.transform.position = Vector2.MoveTowards(character.transform.position, character.target.transform.position, dashSpeed*Time.fixedDeltaTime);
            //once in range deal damage and start CD if kills target reset CD 0.5f is the range in this case
            if (Vector2.Distance(character.transform.position, character.target.transform.position) < 0.5f) {
                character.agent.enabled = true;//renables to allow for pathfinding again
                //deal damage
                character.damage(character.target, amt, true);
                if (character.target.HP < 0) {
                    startCooldown();
                    //reset cd
                    abilityNext = cdReset;
                }
                else {
                    //start cd
                    startCooldown();
                }
            }
            
        }
    }

    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
        description = "Dash towards target dealing " + amt + "PD reset CD if this kills";
    }


}
