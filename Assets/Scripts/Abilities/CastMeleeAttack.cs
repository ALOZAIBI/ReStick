using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;


public class CastMeleeAttack : Ability
{
    //the prefab object that will be instantiated will be the attack

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


    public float selfRootDuration;
    public override void doAbility() {
        if(available && character.selectTarget(targetStrategy, rangeAbility)) {
            calculateAmt();
            playAnimation("castRaise");
        }
    }

    public override void executeAbility() {
        MeleeAttack objAttack = Instantiate(prefabObject, character.transform.position, character.transform.rotation).GetComponent<MeleeAttack>();
        //sets the character to be the caster of this ability
        objAttack.character = character;
        //sets the damage amount
        objAttack.DMG = valueAmt.getAmtValueFromName(this, "Damage");
        //sets the target
        objAttack.target = character.target;
        //tells it this abilityName
        objAttack.castingAbilityName = abilityName;
        if (valueAmt.getAmtValueFromName(this,"BuffDuration") > 0 && buffPrefab == null) {
            Debug.Log("DEBOGAS");
        }
        //if there is a buff in this ability
        if (valueAmt.getAmtValueFromName(this, "BuffDuration") > 0) {
            if (buffPrefab == null)
                throw new System.Exception("NO BUFF PREFAB");
            Buff buff = createBuff();
            //makes the buffs scale with character's inf
            {
                buff.PD = PD;
                if (PD > 0) {
                    buff.PD += valueAmt.getAmtValueFromName(this,"BuffStrength")* PD;
                }

                buff.MD = MD;
                if (MD > 0) {
                    buff.MD += valueAmt.getAmtValueFromName(this,"BuffStrength")* MD;
                }

                buff.INF = INF;
                if (INF > 0) {
                    buff.INF += valueAmt.getAmtValueFromName(this,"BuffStrength")* INF;
                }

                buff.HP = HP;
                if (HP > 0) {
                    buff.HP += valueAmt.getAmtValueFromName(this,"BuffStrength")* HP;
                }

                buff.AS = AS;
                if (AS > 0) {
                    buff.AS += valueAmt.getAmtValueFromName(this,"BuffStrength")* AS;
                }

                buff.CDR = CDR;
                if (CDR > 0) {
                    buff.CDR += valueAmt.getAmtValueFromName(this,"BuffStrength")* CDR;
                }

                buff.MS = MS;
                if (MS > 0) {
                    buff.MS += valueAmt.getAmtValueFromName(this,"BuffStrength") * MS;
                }

                buff.Range = Range;
                if (Range > 0) {
                    buff.Range += valueAmt.getAmtValueFromName(this,"BuffStrength")* Range;
                }

                buff.LS = LS;
                if (LS > 0) {
                    buff.LS += valueAmt.getAmtValueFromName(this,"BuffStrength")* LS;
                }

                buff.size = size;
                if (size > 0) {
                    buff.size += valueAmt.getAmtValueFromName(this,"BuffStrength")* size;
                }

                buff.snare = root;
                buff.silence = silence;
                buff.blind = blind;

                //sets caster and target
                buff.caster = character;
                buff.target = character.target;
                //increases buff duration
                buff.duration = valueAmt.getAmtValueFromName(this,"BuffDuration");
                buff.code = abilityName + character.name;
            }
            objAttack.buff = buff;
        }
        //root self for selfRootDuration
        if (selfRootDuration > 0) {
            Buff buff = createBuff();
            buff.snare = true;
            buff.duration = selfRootDuration;
            buff.caster = character;
            buff.target = character;
            buff.code = abilityName + character.name;
            buff.gameObject.SetActive(true);
            buff.applyBuff();
        }
        startCooldown();
    }
    public override void updateDescription() {
        description = prefabObject.GetComponent<PivotEmpty>().description;

        if (character != null) {
            calculateAmt();
            description += " dealing " + valueAmt.getAmtValueFromName(this, "Damage");
        }
    }
    private void Start() {
        base.Start();
        updateDescription();
    }
    private void FixedUpdate() {
        cooldown();
    }
}
