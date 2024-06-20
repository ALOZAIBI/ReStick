using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloneOnDeath : Item
{

    //Summon 2 weaker clones of the character on death
    [SerializeField]public int numOfClones;
    [SerializeField] public int timesToClone=1;
    [SerializeField]private SimpleFX summonFX;


    public override bool onDeath(Character k = null) {
        if(timesToClone <=0) {
            return false;
        }
        //Debug.Log("On Death " + character.name);
        for(int i = 0; i < numOfClones; i++) {
            //Debug.Log("Iteration " + i);
            Character summoned = character.summon(character, (1f / numOfClones), (1 / 1.2f), 2, true);

            //Decrements this item
            foreach (Item item in summoned.items) {
                if (item is CloneOnDeath) {
                    CloneOnDeath cloneOnDeath = (CloneOnDeath)item;
                    cloneOnDeath.timesToClone--;
                }
            }

            SimpleFX fx = Instantiate(summonFX, summoned.transform.position, summoned.transform.rotation);
            fx.transform.localScale = summoned.transform.localScale;
            fx.keepOnTarget.target = summoned.gameObject;   
        }
        startItemActivation();
        //Debug.Log("End of On Death "+ character.name);

        return false;
    }


}
