using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Ability
{
    [SerializeField] private SimpleFX summonFX;
    private int debugName = 0;
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
        Character charSummoned = character.summon( prefabObject.GetComponent<Character>(), strength);
        charSummoned.name = charSummoned.name + debugName;
        debugName++;
        
        charSummoned.attackTargetStrategy = targetStrategy;
        charSummoned.movementStrategy = (int)Character.MovementStrategies.Default;

        //Create FX on the summoned character
        SimpleFX fx = Instantiate(summonFX, charSummoned.transform.position, charSummoned.transform.rotation);
        fx.transform.localScale = charSummoned.transform.localScale;
        fx.keepOnTarget.target = charSummoned.gameObject;

        //Create FX on the summoner
        SimpleFX fx2 = Instantiate(summonFX, character.transform.position, character.transform.rotation);
        fx2.transform.localScale = character.transform.localScale;
        fx2.keepOnTarget.target = character.gameObject;


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
