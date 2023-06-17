using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotEmpty : MeleeAttack
{
    //this script simply send the info to it's child so that this object can be the prefabObject of abilities despite what we actually want it this guys child. Yeah another fucked explanation but yeah.
    public MeleeAttack child;
    public override void attackHandler() {
        
    }
    private void Start() {
        //assign all variables to child
        child.character = character;
        child.target = target;
        child.DMG = DMG;
        child.speed = speed;
        child.LS = LS;
        child.range= range;
        child.lifetime = lifetime;
        child.buff = buff;

    }
}
