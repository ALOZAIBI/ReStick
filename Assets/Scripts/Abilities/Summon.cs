using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Ability
{
    //Summons the prefabObject and sets up some stats
    public override void doAbility() {
        if (available) {
            calculateAmt();
            //character.selectTarget(targetStrategy);
            //Summons the character in a slightly random position from the casting character
            GameObject objSummoned = Instantiate(prefabObject, character.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), character.transform.rotation);
            Character charSummoned = objSummoned.GetComponent<Character>();
            charSummoned.HPMax += charSummoned.HPMax * amt;
            charSummoned.HP += charSummoned.HP * amt;
            charSummoned.PD += charSummoned.PD * amt;
            charSummoned.MD += charSummoned.MD * amt;
            charSummoned.AS += charSummoned.AS * amt;
            charSummoned.CDR += charSummoned.CDR * amt;
            charSummoned.MS += charSummoned.MS * amt;
            charSummoned.Range += charSummoned.Range * amt;
            charSummoned.LS += charSummoned.LS * amt;

            //Sets the summoned character's team and targetting strategy
            charSummoned.team = character.team;
            charSummoned.attackTargetStrategy = targetStrategy;
            charSummoned.movementTargetStrategy = targetStrategy;
            startCooldown();
        }
    }
    //WIP description
    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /* avoids null character issue*/}
        description = "Summon character";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown();
    }
}
