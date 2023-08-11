using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastAura : Ability
{
    //duration that aura is active per cast
    public float duration;
    //duration remaining
    public float durationRemaining=0;
    public bool active = false;
    //wether it targets enemy or ally or both
    public bool enemy;
    public bool ally;

    public bool damage;
    public bool heal;

    private Aura aura;

    //Buff Values
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

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if (available) {
            calculateAmt();
            playAnimation("castRaise");
        }
    }

    public override void executeAbility() {
        GameObject temp = Instantiate(prefabObject);
        temp.transform.localScale = new Vector3(rangeAbility * 2, rangeAbility * 2, rangeAbility * 2);
        aura = temp.GetComponent<Aura>();
        //if there is a buff in this ability
        if (valueAmt.getAmtValueFromName(this, "BuffDuration") > 0) {
            if (buffPrefab == null)
                throw new System.Exception("NO BUFF PREFAB");
            Buff buff = createBuff();
            //makes the buffs scale with character's inf
            {
                buff.PD = PD;
                if (PD > 0) {
                    buff.PD += valueAmt.getAmtValueFromName(this, "BuffStrength") * PD;
                }

                buff.MD = MD;
                if (MD > 0) {
                    buff.MD += valueAmt.getAmtValueFromName(this, "BuffStrength") * MD;
                }

                buff.INF = INF;
                if (INF > 0) {
                    buff.INF += valueAmt.getAmtValueFromName(this, "BuffStrength") * INF;
                }

                buff.HP = HP;
                if (HP > 0) {
                    buff.HP += valueAmt.getAmtValueFromName(this, "BuffStrength") * HP;
                }

                buff.AS = AS;
                if (AS > 0) {
                    buff.AS += valueAmt.getAmtValueFromName(this, "BuffStrength") * AS;
                }

                buff.CDR = CDR;
                if (CDR > 0) {
                    buff.CDR += valueAmt.getAmtValueFromName(this, "BuffStrength") * CDR;
                }

                buff.MS = MS;
                if (MS > 0) {
                    buff.MS += valueAmt.getAmtValueFromName(this, "BuffStrength") * MS;
                }

                buff.Range = Range;
                if (Range > 0) {
                    buff.Range += valueAmt.getAmtValueFromName(this, "BuffStrength") * Range;
                }

                buff.LS = LS;
                if (LS > 0) {
                    buff.LS += valueAmt.getAmtValueFromName(this, "BuffStrength") * LS;
                }

                buff.size = size;
                if (size > 0) {
                    buff.size += valueAmt.getAmtValueFromName(this, "BuffStrength") * size;
                }

                buff.snare = root;
                buff.silence = silence;
                buff.blind = blind;

                //sets caster and target
                buff.caster = character;
                buff.target = character.target;

                //increases buff duration according to AMT
                buff.duration = valueAmt.getAmtValueFromName(this, "BuffDuration");
                buff.code = abilityName + character.name;
            }
            aura.buff = buff;
        }

        //sets the amt 
        aura.amt = valueAmt.getAmtValueFromName(this, "Amount");
        //sets the caster
        aura.caster = character;
        aura.ally = ally;
        aura.enemy = enemy;
        aura.damage = damage;
        aura.heal = heal;
        aura.castingAbilityName = abilityName;
        prefabObject.SetActive(true);
        startCooldown();
        startActiveDuration();
    }
    public override void updateDescription() {
        description = "";
        if (character == null) {
            if (heal)
                description += "Heals nearby allies ";
            if(damage)
                description += "Deals damage to nearby enemies";
        }
        else {
            calculateAmt();
            if (heal) {
                description += "Heals nearby allies by " + valueAmt.getAmtValueFromName(this,"Amount") + " per second ";
            }
            if(damage)
                description += "Deals " + valueAmt.getAmtValueFromName(this,"Amount") + "per second to nearby enemies";
        }
    }
    public void startActiveDuration() {
        durationRemaining = duration;
    }

    public void decrementDuration() {
        if (durationRemaining > 0) {
            durationRemaining -= Time.fixedDeltaTime;
        }
        else {
            durationRemaining = 0;
            Destroy(aura.gameObject);
        }
    }

    //as the name states
    private void keepAuraObjectOnCaster() {
        aura.transform.position = character.transform.position;
    }
    void FixedUpdate() {
        cooldown();
        try {
            decrementDuration();

            keepAuraObjectOnCaster();
        }
        catch { /*Avoided error when there is no aura instantiated yet. Before ability is done */}
    }
}
