using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOnCast : Item
{
    //The Character prefab to be summoned
    public GameObject summonPrefab;

    [SerializeField]private SimpleFX summonFX;

    //It is activated every second cast
    private int time = 0;


    public override void afterAbility() {
        time++;
        if (time == 2) {
            time = 0;
            float strength = character.INF * 0.005f + character.MD * 0.005f;
            Character summoned = character.summon(summonPrefab.GetComponent<Character>(), strength);

            //Create FX on the summoned character
            SimpleFX fx = Instantiate(summonFX, summoned.transform.position, summoned.transform.rotation);
            fx.transform.localScale = summoned.transform.localScale;
            fx.keepOnTarget.target = summoned.gameObject;

            //Create FX on the summoner
            SimpleFX fx2 = Instantiate(summonFX, character.transform.position, character.transform.rotation);
            fx2.transform.localScale = character.transform.localScale;
            fx2.keepOnTarget.target = character.gameObject;

            startItemActivation();
        }
    }
}
