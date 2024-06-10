using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneOnDeath : Item
{

    //Summon 2 weaker clones of the character on death
    [SerializeField]public int numOfClones;
    [SerializeField] public int timesToClone=1;


    public override void onDeath() {
        if(timesToClone <=0) {
            return;
        }
        for(int i = 0; i < numOfClones; i++) {
            //summons the clone in a position near the summoner
            Character clone = Instantiate(character.gameObject, character.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0), character.transform.rotation).GetComponent<Character>();
            clone.summoned = true;
            clone.summoner = character;

            if (clone.buffs.Count > 0) {
                //the .ToArray is used to prevent the error of modifying the list while iterating through it.
                foreach (Buff tempBuff in clone.buffs.ToArray()) {
                    tempBuff.removeBuffAppliedStats(clone);
                }
            }
            //Clones the abilities
            int index = 0;
            foreach (Ability ability in character.abilities) {
                Ability temp = Instantiate(ability);
                temp.available = true;
                temp.abilityNext = 0;
                clone.abilities[index] = temp;
                index++;
            }

            //Clones the items except this cloning item
            index = 0;
            foreach (Item item in character.items) {
                Item temp = Instantiate(item);
                if (temp is CloneOnDeath) {
                    CloneOnDeath cloneOnDeath = (CloneOnDeath)temp;
                    cloneOnDeath.timesToClone--;
                }
                clone.items[index] = temp;
                index++;
            }

            //Makes the clone weaker
            clone.PD = character.PD / numOfClones;
            clone.MD = character.MD / numOfClones;
            clone.INF = character.INF / numOfClones;
            clone.HP = character.HP / numOfClones;
            clone.HPMax = character.HPMax / numOfClones;

            clone.HP = clone.HPMax;

            //Make the size half of the summoner
            clone.transform.localScale = new Vector3(character.transform.localScale.x / 1.5f, character.transform.localScale.y / 1.5f, character.transform.localScale.z / 1.5f);


            //Adds the clone to the zone
            clone.zone.charactersInside.Add(clone);

        }
        startItemActivation();


    }


}
