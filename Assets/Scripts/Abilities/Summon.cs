using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Ability
{
    public override void Start() {
        base.Start();
        updateDescription();
        if (character != null) {
            calculateAmt();
        }
    }
    //Summons the prefabObject and sets up some stats
    public override bool doAbility() {
        if (available) {
            calculateAmt();
            playAnimation("castRaise");
            return true;
        }
        return false;
    }
    public override void executeAbility() {
        base.executeAbility();
        float strength = valueAmt.getAmtValueFromName(this, "SummonQuality");
        Character charSummoned = Skills.summon(character, prefabObject.GetComponent<Character>(), strength);
        
        charSummoned.attackTargetStrategy = targetStrategy;
        charSummoned.movementStrategy = (int)Character.MovementStrategies.Default;
        startCooldown();
    }
    //WIP description
    public override void updateDescription() {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown();
    }
}
