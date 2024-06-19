using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneOnDeath : Item
{

    //Summon 2 weaker clones of the character on death
    [SerializeField]public int numOfClones;
    [SerializeField] public int timesToClone=1;


    public override bool onDeath(Character k = null) {
        if(timesToClone <=0) {
            return false;
        }
        for(int i = 0; i < numOfClones; i++) {
            //summons the clone in a position near the summoner
            Character clone = Instantiate(character.gameObject, character.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), character.transform.position.z), character.transform.rotation).GetComponent<Character>();
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
                //Debugging just the first guy
                if(i == 0)
                    Debug.Log("Before:"+temp.abilityNext +" " + temp.available);
                //temp.available = true;
                //temp.abilityNext = 0;
                temp.setRandomCD();
                clone.abilities[index] = temp;
                if(i == 0) {
                    Debug.Log("After:" + clone.abilities[index].abilityNext +" " + clone.abilities[index].available); 
                }
                index++;
            }

            //Clones the items except this cloning item
            index = 0;
            foreach (Item item in character.items) {
                Item temp = Instantiate(item);
                temp.reset();
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
            clone.transform.localScale = new Vector3(character.transform.localScale.x / 1.18f, character.transform.localScale.y / 1.18f, character.transform.localScale.z / 1.18f);


            //Adds the clone to the zone
            character.zone.charactersInside.Add(clone);

        }
        startItemActivation();

        return false;
    }


}
