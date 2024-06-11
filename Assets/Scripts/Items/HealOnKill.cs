using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : Item
{
    //How much % of maxHP will this heal
    [SerializeField] private float healPercent;

    [SerializeField] private SimpleFX regenFX;

    public override void onKill() {
        character.HP += character.HPMax * character.killsLastFrame * (healPercent / 100);

        //Instantiate regenFX on character
        SimpleFX fx = Instantiate(regenFX, character.transform.position, Quaternion.identity);
        fx.keepOnTarget.target = character.gameObject;
        startItemActivation();

    }
}
