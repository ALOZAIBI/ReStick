using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Ability
{
    //Summons the prefabObject and sets up some stats
    public override void doAbility() {
        if (available) {
            //character.selectTarget(targetStrategy);
            //Summons the character in a slightly random position from the casting character
            GameObject objSummoned = Instantiate(prefabObject, character.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), character.transform.rotation);
            Character charSummoned = objSummoned.GetComponent<Character>();
            //Sets the summoned character's team and targetting strategy
            charSummoned.team = character.team;
            charSummoned.attackTargetStrategy = targetStrategy;
            charSummoned.movementTargetStrategy = targetStrategy;
            startCooldown();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown();
    }
}
