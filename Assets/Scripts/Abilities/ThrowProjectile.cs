using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : Ability
{
    //this applies buff to everyone that this projectile hits
    public Buff buffPrefab;

    public float PD;
    public float MD;
    public float INF;
    public float HP;
    public float AS;
    public float CDR;
    public float MS;
    public float Range;
    public float LS;

    public float size;

    //used to root silence and blind etc..
    public bool root;
    public bool silence;
    public bool blind;

    public float buffDuration;

    //Summons a projectile
    //can do cool shit depending on the projectile
    //for example a projectile that throws an AOE that heals
    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if (available&& character.selectTarget(targetStrategy,rangeAbility)) {
            calculateAmt();
            //Debug.Log("ABILITY DONE WHAT");
            

            //creates the projectile
            GameObject objProjectile = Instantiate(prefabObject, character.transform.position, character.transform.rotation);
            Projectile projectile = objProjectile.GetComponent<Projectile>();
            //sets the shooter to be the caster of this ability
            projectile.shooter = character;
            //sets the damage amount
            projectile.DMG = amt;
            //sets the target
            projectile.target = character.target;
            //tells it this abilityName
            projectile.castingAbilityName = abilityName;
            if (buffDuration > 0 && buffPrefab == null) {
                Debug.Log("DEBOGAS");
            }
            //if there is a buff in this ability
            if (buffDuration > 0) {
                if(buffPrefab == null)
                    throw new System.Exception("NO BUFF PREFAB");
                Buff buff = createBuff();
                //makes the buffs scale with character's inf
                {
                    buff.PD = PD;
                    if (PD > 0) {
                        buff.PD += character.INF * 0.4f * PD;
                    }

                    buff.MD = MD;
                    if (MD > 0) {
                        buff.MD += character.INF * 0.4f * MD;
                    }

                    buff.INF = INF;
                    if (INF > 0) {
                        buff.INF += character.INF * 0.4f * INF;
                    }

                    buff.HP = HP;
                    if (HP > 0) {
                        buff.HP += character.INF * 0.4f * HP;
                    }

                    buff.AS = AS;
                    if (AS > 0) {
                        buff.AS += character.INF * 0.4f * AS;
                    }

                    buff.CDR = CDR;
                    if (CDR > 0) {
                        buff.CDR += character.INF * 0.4f * CDR;
                    }

                    buff.MS = MS;
                    if (MS > 0) {
                        buff.MS += character.INF * 0.05f * MS;
                    }

                    buff.Range = Range;
                    if (Range > 0) {
                        buff.Range += character.INF * 0.4f * Range;
                    }

                    buff.LS = LS;
                    if (LS > 0) {
                        buff.LS += character.INF * 0.4f * LS;
                    }

                    buff.size = size;
                    if (size > 0) {
                        buff.size += character.INF * 0.4f * size;
                    }

                    buff.snare = root;
                    buff.silence = silence;
                    buff.blind = blind;

                    //sets caster and target
                    buff.caster = character;
                    buff.target = character.target;
                    //increases buff duration according to AMT
                    buff.duration = buffDuration;
                    buff.duration += character.INF * 0.4f / 10;
                    buff.code = abilityName + character.name;
                }
                projectile.buff = buff;
            }
            

            ////sets the projectiles direction
            //projectile.direction = character.target.transform.position - character.transform.position;
            ////normalises the direction so that projectile speed won't be affected by target distance
            //projectile.direction = (10 * projectile.direction).normalized;
            startCooldown();
        }

    }
    public override void updateDescription() {
        try {
            calculateAmt();
        }
        catch { /*Debug.Log("Calculate AMT didn't work"); avoids null character issue*/}
        description = "Throws a thing that deals " + amt + " DMG to all characters within";
    }
    private Buff createBuff() {
        Buff buff = Instantiate(buffPrefab).GetComponent<Buff>();
        buff.gameObject.SetActive(false);
        return buff;
    }
    private void FixedUpdate() {
        cooldown();
    }


}
