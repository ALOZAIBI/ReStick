using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : Item
{
    //How much % of maxHP will this heal
    [SerializeField] private float healPercent;

    [SerializeField] private float timeSinceLastHeal;
    [SerializeField] private float timeBetweenHeals;

    public override void onZoneStart() {
        timeSinceLastHeal = 0;
    }

    public override void continuous() {
        timeSinceLastHeal += Time.fixedDeltaTime;
        if (timeSinceLastHeal >= timeBetweenHeals) {
            timeSinceLastHeal = 0;

            float healAmount = character.HPMax * healPercent;
            character.HP += healAmount;
            if (character.HP > character.HPMax) {
                character.HP = character.HPMax;
            }
        }
    }

}