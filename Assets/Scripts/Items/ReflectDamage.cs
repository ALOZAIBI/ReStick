using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectDamage : Item
{
    public float percentDamageReflected;
    public override void onTakeDamage(Character theDamager,float damageAmount) {
        character.damage(theDamager, damageAmount * percentDamageReflected,0.1f);
        startItemActivation();
    }
}
