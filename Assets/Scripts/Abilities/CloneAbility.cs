using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAbility : Ability
{
    public override void doAbility() {
        if(available) {
            calculateAmt();
            playAnimation("castRaise");
        }
    }
    public override void executeAbility() {
        character.selectTarget(targetStrategy);
        //cooldown is set before the ability is executed so that if this character is cloned the clone ability won't be ready again to not cause cloning to go to infinity instantly
        startCooldown();
        //summons the clone in a position near the summoner
        Character clone = Instantiate(character.target.gameObject, character.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0), character.transform.rotation).GetComponent<Character>();
        clone.summoned = true;
        clone.summoner = character;

        //clones the abilities
        int index = 0;
        foreach (Ability ability in character.target.abilities) {
            Ability temp = Instantiate(ability);
            //the clone ability's cd will not be ready so that the cloning won't go to infinity instantly
            if (temp is CloneAbility) {
                temp.startCooldown();
            }
            //however all other abilities will be ready.
            else {
                temp.available = true;
                temp.abilityNext = 0;
            }
            clone.abilities[index] = temp;
            index++;
        }
        if (clone.team != character.team) {
            //if the clone is of an enemy make it allied.
            clone.team = character.team;
            //maybe decrease the amount to make enemy clones even weaker otherwise OP innit.
            valueAmt.getAmtValueFromName(this,"CloneQuality");
        }

        //Changes movement strategy to be appropriate for a clone
        if(clone.movementStrategy == (int)Character.MovementStrategies.DontMove) {
            clone.movementStrategy = (int)Character.MovementStrategies.Default;
        }

        clone.PD = clone.PD * valueAmt.getAmtValueFromName(this, "CloneQuality");
        clone.MD = clone.MD * valueAmt.getAmtValueFromName(this, "CloneQuality");
        clone.INF = clone.INF * valueAmt.getAmtValueFromName(this, "CloneQuality");
        clone.HP = clone.HP * valueAmt.getAmtValueFromName(this, "CloneQuality");
        clone.HPMax = clone.HPMax * valueAmt.getAmtValueFromName(this, "CloneQuality");
        clone.name = character.target.name + " Clone";
        SpriteRenderer sprite = clone.GetComponent<SpriteRenderer>();
        //darken the color of the clone
        sprite.color = new Color(sprite.color.r * 0.5f, sprite.color.g * 0.5f, sprite.color.b * 0.5f);

        //adds to zone
        clone.zone.charactersInside.Add(clone);
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
