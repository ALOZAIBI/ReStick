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
        //Summons the character in a slightly random position from the casting character
        GameObject objSummoned = Instantiate(prefabObject, character.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), character.transform.rotation);
        Character charSummoned = objSummoned.GetComponent<Character>();
        charSummoned.HPMax += charSummoned.HPMax * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.HP += charSummoned.HP * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.PD += charSummoned.PD * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.MD += charSummoned.MD * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.INF += charSummoned.INF * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.AS += charSummoned.AS * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.CDR += charSummoned.CDR * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.MS += charSummoned.MS * valueAmt.getAmtValueFromName(this, "SummonQuality") * 0.2f;
        if (charSummoned.Range > 1)//only increase range if the char summoned is ranged
            charSummoned.Range += charSummoned.Range * valueAmt.getAmtValueFromName(this, "SummonQuality");
        charSummoned.LS += charSummoned.LS * valueAmt.getAmtValueFromName(this, "SummonQuality");

        charSummoned.summoned = true;
        charSummoned.summoner = character;
        //Sets the summoned character's team and targetting strategy
        charSummoned.team = character.team;
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
