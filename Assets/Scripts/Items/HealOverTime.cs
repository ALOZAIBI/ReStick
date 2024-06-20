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


    //For example in devil's contract we want to start round with atleast 80% hp
    [SerializeField] private float roundStartSetHPPercentMin = 0;
    public override void onZoneStart() {
        timeSinceLastHeal = 0;

        //If the character is below the minimum hp, set it to the minimum hp
        if(character.HP < character.HPMax * roundStartSetHPPercentMin) {
            character.HP = character.HPMax * roundStartSetHPPercentMin;
        }
    }

    public override void continuous() {
        timeSinceLastHeal += Time.fixedDeltaTime;
        if (timeSinceLastHeal >= timeBetweenHeals) {
            timeSinceLastHeal = 0;

            float healAmount = character.HPMax * healPercent;
            if (healAmount > 0) { 
                character.HP += healAmount;
                if (character.HP > character.HPMax) {
                    character.HP = character.HPMax;
                }
            }
            else {
                character.damage(character, -healAmount,false);
            }
            //This can damage self like in devil's contract
            if (regenFX != null) {
                //Instantiate regenFX on character
                SimpleFX fx = Instantiate(regenFX, character.transform.position, Quaternion.identity);
                fx.keepOnTarget.target = character.gameObject;
            }
            startItemActivation();
        }
    }

}
