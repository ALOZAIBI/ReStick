using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTarget : Ability
{
    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {      //Added the check if interruptible to not make this function called every frame.

        if (character.animationManager.interruptible && available) {
            //If there is a character within range that is not at full HP, select it as a target
            if (character.selectTarget(targetStrategy, rangeAbility,excludeTargets())) {
                Debug.Log("Character selected is " + character.target.name);
                calculateAmt();
                playAnimation("castRaise");
            }
        }
    }

    public override void executeAbility() {
        Debug.Log("Heal Target is" + character.target.name);
        //heals the target
        character.target.HP += valueAmt.getAmtValueFromName(this, "Heal");
        //creates the healing effect
        KeepOnTarget fx = Instantiate(prefabObject, character.target.transform.position, Quaternion.identity).GetComponent<KeepOnTarget>();
        fx.target = character.target.gameObject;
        Destroy(fx.gameObject, 1.3f);

        startCooldown();
    }

    public override List<Character> excludeTargets() {
        //List of characters that are at full HP that will then be excluded from selection
        List<Character> fullHP = new List<Character>();
        foreach (Character c in character.zone.charactersInside) {
            //If it's an ally and it's at full HP, add it to the list
            if (c.team == character.team && Mathf.Approximately(c.HP, c.HPMax)) {
                Debug.Log("Added " + c.name + " to fullHP");
                fullHP.Add(c);
            }
        }
        return fullHP;
    }
    public override void updateDescription() {
        if (character != null) {
            calculateAmt();
            description = "Heals target by " + valueAmt.getAmtValueFromName(this, "Heal");
        }
        else
            description = "Heals target";
        
    }

    private void FixedUpdate() {
        cooldown();
    }
}
