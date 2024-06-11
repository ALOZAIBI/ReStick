using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : Item
{
    //How much % of maxHP will this heal
    [SerializeField] private float healPercent;

    [SerializeField] private float timeSinceLastHeal;
    [SerializeField] private float timeBetweenHeals;

    [SerializeField] private SimpleFX regenFX;

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
            if (regenFX != null) {
                //Instantiate regenFX on character
                SimpleFX fx = Instantiate(regenFX, character.transform.position, Quaternion.identity);
                fx.keepOnTarget.target = character.gameObject;
            }
            startItemActivation();
        }
    }

}
