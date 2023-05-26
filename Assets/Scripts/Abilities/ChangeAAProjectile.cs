using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAAProjectile : Ability {
    //once the aa has been changed set this to true
    public bool applied;
    public override void doAbility() {
        //sets the Character's projectile
        if (!applied) {
            character.usesProjectile = true;
            character.projectile = prefabObject;
            applied = true;
        }
    }
    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void updateDescription() {
        //this is manually set in the editor
    }
}
