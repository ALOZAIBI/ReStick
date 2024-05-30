using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOnKill : Item
{
    //How much % of maxHP will this heal
    [SerializeField] private float healPercent;

    public override void onKill() {
        character.HP += character.HPMax * character.killsLastFrame * (healPercent / 100);
    }
}
