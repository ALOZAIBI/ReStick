using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneAbility : Ability
{
    public override void doAbility() {
        if(available) {
            calculateAmt();
            character.selectTarget(targetStrategy);
            //cooldown is set before the ability is executed so that if this character is cloned the clone ability won't be ready again to not cause cloning to go to infinity instantly
            startCooldown();
            //summons the clone in a position near the summoner
            Character clone =Instantiate(character.target.gameObject, character.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0), character.transform.rotation).GetComponent<Character>();
            clone.summoned = true;
            clone.summoner = character;
            
            //clones the abilities
            int index = 0;
            foreach (Ability ability in character.target.abilities) {
                Ability temp = Instantiate(ability);
                //the clone ability's cd will not be ready so that the cloning won't go to infinity instantly
                if (temp is CloneAbility) {
                    //temp.startCooldown();
                    Debug.Log("CloneAbility cd is set to "+temp.abilityNext+temp.available);
                }
                clone.abilities[index] = temp;
                index++;
            }
            if(clone.team != character.team) {
                //if the clone is of an enemy make it allied.
                clone.team = character.team;
                //decrease the amount to make enemy clones even weaker otherwise OP innit.
                amt *= 0.6f;
            }
            clone.PD = clone.PD*amt;
            clone.MD = clone.MD* amt;
            clone.INF = clone.INF * amt;
            clone.HP = clone.HP * amt;
            clone.HPMax = clone.HPMax * amt;
            clone.name = character.target.name + " Clone";
            SpriteRenderer sprite = clone.GetComponent<SpriteRenderer>();
            sprite.color = Color.gray;
            
        }
    }

    public override void updateDescription() {
        description = "Clone a weaker version of my target";
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
