using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{
    //how much cd will be at once it's reset
    private float cdReset=0.03f;
    [SerializeField] private bool resetOnKill;
    [SerializeField] private string initDescription;

    //Maybe add this in the future it is taken frmo the initial stick project check my gitlab
    //public float resetPossibility = 0.7f;   //target has 0.7 seconds to die after dash for the dash to reset

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

    private void FixedUpdate() {
        cooldown();
    }
    public override void doAbility() {
        if (available&& character.selectTarget(targetStrategy,rangeAbility) && canUseDash()) {
            calculateAmt();
            playAnimation("castDash");
            //executeAbility();
            
        }
        //This is done since the dash animation does castEventDoNotInterrupt which prevents the above playAnimation from triggering. So we are triggering it here once the playAnimation has been triggered atleasst once
        //It is done outside the initial if statement so that we can go through the if Dead or null check. Becaise if there are no characters within range then the ability won't be executed and won't go through the check.
        if (character.currentDashingAbility == this) {
            executeAbility();
        }
    }

    public override void executeAbility() {
        //If my target is dead or null cancel the ability
        if (character.target == null || !character.target.alive) {
            character.agent.enabled = true;
            character.animationManager.forceStop();
            Debug.Log("Cancel dashAbility");
            return;
        }
        character.currentDashingAbility = this;
        //dashes to target
        character.agent.enabled = false;//to allow the target to dash through obstacles
        character.transform.position = Vector2.MoveTowards(character.transform.position, character.target.transform.position, valueAmt.getAmtValueFromName(this, "DashSpeed") * Time.fixedDeltaTime);
        //once in range deal damage and start CD if kills target reset CD 0.5f is the range in this case
        if (Vector2.Distance(character.transform.position, character.target.transform.position) < 0.5f) {
            character.agent.enabled = true;//renables to allow for pathfinding again
                                           //deal damage
            character.damage(character.target, valueAmt.getAmtValueFromName(this, "Damage"), true);
            applyHitFX(character.target);
            if (resetOnKill && character.target.HP < 0) {
                startCooldown();
                character.animationManager.forceStop();
                //reset cd
                abilityNext = cdReset;
            }
            else {
                //start cd
                startCooldown();
                character.animationManager.forceStop();
            }
            //Apply buffs
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
                buff.applyBuff();
            }
        }

    }

    public override void updateDescription() {
        if (character == null) {
            description = initDescription;
            if(resetOnKill) {
                description += " If this kills target reset CD";
            }
        }
        else {
            calculateAmt();
            description = initDescription + valueAmt.getAmtValueFromName(this, "Damage");
            if(resetOnKill) {
                description += " If this kills target reset CD";
            }
        }
    }


}
