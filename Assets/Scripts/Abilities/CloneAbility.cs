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
            //summons the clone in a position near the target
            Character clone =Instantiate(character.target.gameObject, character.target.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), character.transform.rotation).GetComponent<Character>();
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
            //amt would usually be less than 100 so usually the clone would be weaker than the character cloned unless the caster has an insane MD
            clone.PD *= amt / 100;
            clone.MD *= amt / 100;
            clone.INF *= amt / 100;
            clone.AS *= amt / 100;
            clone.HP *= amt / 100;
            //decrease the CD of clone by flat amount but make sure it doesnt go below 0
            clone.CDR=Mathf.Clamp(clone.CDR -0.1f, 0, 5000);
            clone.HPMax *= amt / 100;
            clone.name = character.target.name + " Clone";
            SpriteRenderer sprite = clone.GetComponent<SpriteRenderer>();
            sprite.color = Color.gray;
            
        }
    }

    public override void updateDescription() {
        description = "Clone a weaker version of my target";
    }

    // Start is called before the first frame update
    void Start()
    {
        updateDescription();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown();
    }
}
