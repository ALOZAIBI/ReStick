using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//The first time I would die revive me instead
public class ReviveOnDeath : Item
{
    [SerializeField] private bool revived = false;
    public override void onDeath(Character c = null) {
        //If not already revived and character is dead (We check if dead since we might have multiple items with this effect and we wouldn't want them all to be used to revive just once)
        if (!revived && !character.alive) {
            Debug.Log("Supposed to revive");
            startItemActivation();
            character.alive = true;
            character.HP = character.HPMax;
            revived = true;
        }
    }

    public override void reset() {
        revived = false;
    }

}
