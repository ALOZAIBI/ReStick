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

    [SerializeField] private string initDescription;

    public override void Start() {
        base.Start();
        updateDescription();
    }
    public override bool doAbility() {
        if (available) {
            calculateAmt();
            playAnimation("castRaise");
            return true;
        }
        return false;
    }

    public override void executeAbility() {
        base.executeAbility();
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
        aura.damage = damage;
        aura.heal = heal;
        aura.castingAbilityName = abilityName;
        prefabObject.SetActive(true);
        startCooldown();
        startActiveDuration();
    }

    ////This is called from other abilities to start the aura (the aura should be saved in the prefabObject of the ability)
    //public static void castAura(this Ability ability) {
    //    GameObject temp = Instantiate(ability.prefabObject);
    //    Aura aura = temp.GetComponent<Aura>();
    //    aura.transform.localScale = new Vector3(ability.rangeAbility * 2, ability.rangeAbility * 2, ability.rangeAbility * 2);
    //    //if there is a buff in this ability
    //    if (ability.valueAmt.getAmtValueFromName(ability, "BuffDuration") > 0) {
    //        if (ability.buffPrefab == null)
    //            throw new System.Exception("NO BUFF PREFAB");
    //        Buff buff = ability.createBuff();
    //        //makes the buffs scale with character's inf
    //        {
    //            buff.PD = ability.PD;
    //            if (ability.PD > 0) {
    //                buff.PD += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.PD;
    //            }

    //            buff.MD = ability.MD;
    //            if (ability.MD > 0) {
    //                buff.MD += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.MD;
    //            }

    //            buff.INF = ability.INF;
    //            if (ability.INF > 0) {
    //                buff.INF += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.INF;
    //            }

    //            buff.HP = ability.HP;
    //            if (ability.HP > 0) {
    //                buff.HP += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.HP;
    //            }

    //            buff.AS = ability.AS;
    //            if (ability.AS > 0) {
    //                buff.AS += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.AS;
    //            }

    //            buff.CDR = ability.CDR;
    //            if (ability.CDR > 0) {
    //                buff.CDR += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.CDR;
    //            }

    //            buff.MS = ability.MS;
    //            if (ability.MS > 0) {
    //                buff.MS += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.MS;
    //            }

    //            buff.Range = ability.Range;
    //            if (ability.Range > 0) {
    //                buff.Range += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.Range;
    //            }

    //            buff.LS = ability.LS;
    //            if (ability.LS > 0) {
    //                buff.LS += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.LS;
    //            }

    //            buff.size = ability.size;
    //            if (ability.size > 0) {
    //                buff.size += ability.valueAmt.getAmtValueFromName(ability, "BuffStrength") * ability.size;
    //            }

    //            buff.snare = ability.root;
    //            buff.silence = ability.silence;
    //            buff.blind = ability.blind;

    //            //sets caster and target
    //            buff.caster = ability.character;
    //            buff.target = ability.character.target;

    //            //increases buff duration according to AMT
    //            buff.duration = ability.valueAmt.getAmtValueFromName(ability, "BuffDuration");
    //            buff.code = ability.abilityName + ability.character.name;
    //        }
    //        aura.buff = buff;
    //    }

    //    //sets the amt 
    //    aura.amt = ability.valueAmt.getAmtValueFromName(ability, "Amount");
    //    //sets the caster
    //    aura.caster = ability.character;
    //    //This will be set in the abilitie's aura prefab
    //    //aura.damage = damage;
    //    //aura.heal = heal;
    //    aura.castingAbilityName = ability.abilityName;
    //    ability.prefabObject.SetActive(true);
    //}
    public override void updateDescription() {
        description = initDescription;
        if (character == null) {
            //if (heal)
            //    description += "Heals nearby allies ";
            //if(damage)
            //    description += "Deals damage to nearby enemies";
            if(buffPrefab != null) {
                description += getDescription();
            }
        }
        else {
            calculateAmt();
            if (heal) {
                description += " by " + valueAmt.getAmtValueFromName(this,"Amount") + " per second.";
            }
            if(damage)
                description += " dealing " + valueAmt.getAmtValueFromName(this,"Amount") + "per second.";

            if (buffPrefab != null) {
                description += getDescription(valueAmt.getAmtValueFromName(this, "BuffStrength"));
            }
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

    public string getDescription(float buffStrength) {
        string description = " Gives";
        if (PD != 0) {
            description += " " + (PD + PD * buffStrength).ToString("F1") + " PWR";
        }
        if (MD != 0) {
            description += " " + (MD + MD * buffStrength).ToString("F1") + " MGC";
        }
        if (INF != 0) {
            description += " " + (INF + INF * buffStrength).ToString("F1") + " INF";
        }
        if (HP != 0) {
            description += " " + (HP + HP * buffStrength).ToString("F1") + " HP";
        }
        if (AS != 0) {
            description += " " + (AS + AS * buffStrength).ToString("F1") + " AS";
        }
        if (CDR != 0) {
            description += " " + (CDR + CDR * buffStrength).ToString("F1") + " CDR";
        }
        if (MS != 0) {
            description += " " + (MS + MS * buffStrength).ToString("F1") + " SPD";
        }
        if (Range != 0) {
            description += " " + (Range + Range * buffStrength).ToString("F1") + " Range";
        }
        if (LS != 0) {
            description += " " + (LS + LS * buffStrength).ToString("F1") + " LS";
        }
        if (root) {
            description += " Root";
        }
        if (silence) {
            description += " Silence";
        }
        if (blind) {
            description += " Blind";
        }

        return description;
    }
    public string getDescription() {
        string description = "Gives";
        if (PD != 0) {
            description += " " + PD + " PWR";
        }
        if (MD != 0) {
            description += " " + MD + " MGC";
        }
        if (INF != 0) {
            description += " " + INF + " INF";
        }
        if (HP != 0) {
            description += " " + HP + " HP";
        }
        if (AS != 0) {
            description += " " + AS + " AS";
        }
        if (CDR != 0) {
            description += " " + CDR + " CDR";
        }
        if (MS != 0) {
            description += " " + MS + " SPD";
        }
        if (Range != 0) {
            description += " " + Range + " Range";
        }
        if (LS != 0) {
            description += " " + LS + " LS";
        }
        if (root) {
            description += " Root";
        }
        if (silence) {
            description += " Silence";
        }
        if (blind) {
            description += " Blind";
        }

        return description;
    }
}
