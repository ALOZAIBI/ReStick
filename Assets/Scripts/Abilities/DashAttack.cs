using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{
    //how much cd will be at once it's reset
    private float cdReset=0.03f;

    //Maybe add this in the future it is taken frmo the initial stick project check my gitlab
    //public float resetPossibility = 0.7f;   //target has 0.7 seconds to die after dash for the dash to reset
    public override void Start() {
        base.Start();
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
            character.transform.position = Vector2.MoveTowards(character.transform.position, character.target.transform.position, valueAmt.getAmtValueFromName(this,"DashSpeed")*Time.fixedDeltaTime);
            //once in range deal damage and start CD if kills target reset CD 0.5f is the range in this case
            if (Vector2.Distance(character.transform.position, character.target.transform.position) < 0.5f) {
                character.agent.enabled = true;//renables to allow for pathfinding again
                //deal damage
                character.damage(character.target, valueAmt.getAmtValueFromName(this, "Damage"), true);
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
        if (character == null) {
            description = "Dash towards target and strike it,Resets CD on kill";
        }
        else {
            calculateAmt();
            description = "Dash towards target dealing " + valueAmt.getAmtValueFromName(this,"Damage") + "Damage reset CD if this kills";
        }
    }


}
