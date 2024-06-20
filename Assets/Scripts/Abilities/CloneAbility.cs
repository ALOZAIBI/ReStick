using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAbility : Ability
{
    [SerializeField]private SimpleFX summonFX;
    public override bool doAbility() {
        if(available) {
            calculateAmt();
            playAnimation("castRaise");
            return true;
        }
        return false;
    }
    public override void executeAbility() {
        base.executeAbility();
        character.selectTarget(targetStrategy);
        //cooldown is set before the ability is executed so that if this character is cloned the clone ability won't be ready again to not cause cloning to go to infinity instantly
        startCooldown();
        Character clone = character.summon(character.target,valueAmt.getAmtValueFromName(this, "CloneQuality"));


        //Ensures that this ability is set on CD for the clone
        foreach (Ability ability in clone.abilities) {
            if (ability is CloneAbility) {
                ability.startCooldown();
            }
        }

        //Create FX on the summoned character
        SimpleFX fx = Instantiate(summonFX, clone.transform.position, clone.transform.rotation);
        fx.transform.localScale = clone.transform.localScale;
        fx.keepOnTarget.target = clone.gameObject;

        //Create FX on the summoner
        SimpleFX fx2 = Instantiate(summonFX, character.transform.position, character.transform.rotation);
        fx2.transform.localScale = character.transform.localScale;
        fx2.keepOnTarget.target = character.gameObject;


        SpriteRenderer sprite = clone.GetComponent<SpriteRenderer>();
        //darken the color of the clone
        sprite.color = new Color(sprite.color.r * 0.5f, sprite.color.g * 0.5f, sprite.color.b * 0.5f);

    }
    public override void updateDescription() {
        if (character == null)
            description = "Clone a weaker version of my target";
        else {
            calculateAmt();
            description = "Clone a target with " + valueAmt.getAmtValueFromName(this, "CloneQuality") * 100 + "% of its stats";
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        updateDescription();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown();
    }
}
