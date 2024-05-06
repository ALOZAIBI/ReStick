using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Similar to throwProjectile, but this is a lot more customizable
public class LaunchProjectiles : Ability
{
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

    public float initSize;
    public float growSpeed;

    public float numProjectiles;
    //At what angle width will the projectiles be launched
    public float angle;
    public float delayBetweenProjectiles = 0;


    //These values need to be reset once the ability is done
    private float numProjectilesLaunched = 0;
    private float delaySinceLastProjectile = 0;

    //So that the doAbility can be called even if the target went out of range
    //___________But what will it be targetting then?_____________
    private bool startedAbility = false;

    public override void Start() {
        base.Start();
        updateDescription();
    }

    public override void doAbility() {
        if (available && character.selectTarget(targetStrategy, rangeAbility)) {
            calculateAmt();
            if (!startedAbility) {
                playAnimation("castRaise");
                startedAbility = true;
            }
            lockedTarget = character.target;
        }
    }

    private void createProjectile() {

        //creates the projectile
        GameObject objProjectile = Instantiate(prefabObject, character.transform.position, character.transform.rotation);
        Projectile projectile = objProjectile.GetComponent<Projectile>();
        //sets the shooter to be the caster of this ability
        projectile.shooter = character;
        //sets the damage amount 
        projectile.DMG = valueAmt.getAmtValueFromName(this, "Damage");

        projectile.initSize = initSize;
        projectile.targetSize = projectile.transform.localScale.x;
        projectile.growSpeed = growSpeed;
        projectile.target = lockedTarget;
        projectile.angle();
        Debug.Log("Projectile has no target" + projectile.name + projectile.shooter);
        //tells it this abilityName
        projectile.castingAbilityName = abilityName;
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
            projectile.buff = buff;
        }
    }
}
