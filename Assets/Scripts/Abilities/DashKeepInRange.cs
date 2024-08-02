using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashKeepInRange : Ability
{
    [SerializeField]private Vector2 directionOffset;
    private bool offsetSet = false;
    public override void Start() {
        base.Start();
        updateDescription();
    }

    private void FixedUpdate() {
        cooldown();
    }

    public override bool doAbility() {
        bool done = false;
        rangeAbility = character.Range + valueAmt.getAmtValueFromName(this, "Range");
        //If available and there is a character within the ability range
        if (available && character.selectTarget(targetStrategy, rangeAbility) && canUseDash()) {
            startAbilityActivation();
            calculateAmt();
            playAnimation("castDash");
            done = true;
            if (!offsetSet) {
                //Get a random vector2 that will be used to offset the character's position
                directionOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                //Normalize the vector2
                directionOffset.Normalize();
                offsetSet = true;
            }
        }
        //This is done since the dash animation does castEventDoNotInterrupt which prevents the above playAnimation from triggering. So we are triggering it here once the playAnimation has been triggered atleasst once
        //It is done outside the initial if statement so that we can go through the if Dead or null check. Becaise if there are no characters within range then the ability won't be executed and won't go through the check.
        if (character.CurrentDashingAbility == this) {
            executeAbility();
            done = true;
        }
        return done;
    }

    public override void executeAbility() {
        //If the character is within maxRange of the enemies then stop dashing
        float minDistance = 1000;
        foreach (Character c in character.zone.charactersInside) {
            if (c.team != character.team) {

                float distance = Vector2.Distance(c.transform.position, character.transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                }
            }
        }
        Debug.Log("Min distance is"+minDistance);
        //The -0.3f is to make sure that the character is slightly within range
        if(minDistance >= character.Range - 0.3f && minDistance <= character.Range + 0.3f) {
            character.agent.enabled = true;
            character.animationManager.forceStop();
            startCooldown();
            return;
        }
        character.CurrentDashingAbility = this;
        //To allow the target to dash through obstacles
        character.agent.enabled = false;
        //Move In the direction
        character.transform.position = Vector2.MoveTowards(character.transform.position, character.transform.position + (Vector3)directionOffset, valueAmt.getAmtValueFromName(this, "Speed") * Time.fixedDeltaTime);

    }
    public override void updateDescription() {
        description = "Dash until the enemies are at max range";
    }

    public override void reset() {
        offsetSet = false;
    }

}
