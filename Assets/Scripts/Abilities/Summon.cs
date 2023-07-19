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
    public override void doAbility() {
        if (available) {
            calculateAmt();
            playAnimation("castRaise");
        }
    }
    public override void executeAbility() {
        //Summons the character in a slightly random position from the casting character
        GameObject objSummoned = Instantiate(prefabObject, character.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), character.transform.rotation);
        Character charSummoned = objSummoned.GetComponent<Character>();
        charSummoned.HPMax += charSummoned.HPMax * amt;
        charSummoned.HP += charSummoned.HP * amt;
        charSummoned.PD += charSummoned.PD * amt;
        charSummoned.MD += charSummoned.MD * amt;
        charSummoned.INF += charSummoned.INF * amt;
        charSummoned.AS += charSummoned.AS * amt;
        charSummoned.CDR += charSummoned.CDR * amt;
        charSummoned.MS += charSummoned.MS * amt * 0.2f;
        if (charSummoned.Range > 1)//only increase range if the char summoned is ranged
            charSummoned.Range += charSummoned.Range * amt;
        charSummoned.LS += charSummoned.LS * amt;

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
