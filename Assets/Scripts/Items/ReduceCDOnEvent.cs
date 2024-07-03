using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCDOnEvent : Item
{
    [SerializeField] private bool eventKill;
    [SerializeField] private bool eventAttack;
    [SerializeField] private bool eventAbility;

    [SerializeField] private float reduction;

    public override void onKill() {
        if (eventKill) {
            reduceCD();
        }
    }

    public override void afterAttack() {
        if (eventAttack) {
            reduceCD();
        }
    }

    public override void afterAbility() {
        if (eventAbility) {
            reduceCD();
        }
    }

    //Reduce the CD of all abilities of the character by reduction seconds
    private void reduceCD() {
        foreach(Ability ability in character.abilities) {
            ability.abilityNext -= reduction;
        }
        startItemActivation();
    }
}
