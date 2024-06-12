using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    public float numProjectiles=1;
    //At what angle width will the projectiles be launched
    public float angle=0;
    public float delayBetweenProjectiles = 0;

    //How many waves of projectiles will be launched
    public float numWaves = 1;
    public float delayBetweenWaves = 1;


    //These values need to be reset once the ability is done
    [SerializeField]private float numProjectilesLaunched = 0;
    private float delaySinceLastProjectile = 0;
    [SerializeField] private float numWavesLaunched = 0;
    private float delaySinceLastWave = 0;


    //So that the doAbility can be continuously called until all projecqtiles are launched or if there are no targets
    private bool abilityStarted = false;

    public override void Start() {
        base.Start();
        updateDescription();

        //The delay between waves should be more than the delay between projectiles * numProjectiles\
        if (delayBetweenWaves < delayBetweenProjectiles * numProjectiles)
            throw new System.Exception("The delay between waves should be more than the delay between projectiles * numProjectiles");
    }

    public override void executeAbility() {
        base.executeAbility();
        abilityStarted = true;
        //Subsequent waves don't require the animation to be finished to fire
        executeAbilityOnEvent = false;
    }

    public override bool doAbility() {
        if (abilityStarted||(available && character.selectTarget(targetStrategy, rangeAbility))) {
            calculateAmt();
            lockedTarget = character.target;
            //So that the animation is only played once
            if (!abilityStarted) {
                reset();
                playAnimation("castRaise");
                //So that the wave is launched immediately
                delaySinceLastWave = delayBetweenWaves;
            }
            //After animation is palyed commence the ability
            else {
                if (delaySinceLastWave >= delayBetweenWaves || numWaves == 1) {

                    //If the delay has passed since the last projectile was launched
                    if (delaySinceLastProjectile >= delayBetweenProjectiles) {
                        playAnimation("castRaise");
                        //If the angle is 360, first projectile will always be launched at 0
                        if (angle == 360 && numProjectilesLaunched == 0) {
                            startAbilityActivation();
                            createProjectile(0);
                        }
                        else {
                            //The angle starts from the left of the target and goes to the right
                            float angleBetweenProjectiles = numProjectiles > 1 ? angle / (numProjectiles - 1) : 0;
                            float angleAwayFromCenter = angle / 2 + -angleBetweenProjectiles * numProjectilesLaunched;
                            createProjectile(angleAwayFromCenter);
                            startAbilityActivation();
                        }
                        delaySinceLastProjectile = 0;
                        numProjectilesLaunched++;
                    }

                    //all projectiles of this wave have been launched
                    if (numProjectilesLaunched == numProjectiles) {
                        numProjectilesLaunched = 0;
                        delaySinceLastProjectile = 0;
                        delaySinceLastWave = 0;
                        numWavesLaunched++;
                    }
                }

                //What would happen if level is reloaded before all projectiles are launched??
                //Once all waves are launched, the ability is done
                if (numWavesLaunched == numWaves) {
                    abilityStarted = false;
                    startCooldown();
                    //In case the animation is still playing we stop it so that it doesn't set abilityStarted to true again
                    character.animationManager.forceStop();
                }
            }
            return true;
        }
        return false;
    }

    private void createProjectile(float angle) {

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
        projectile.setAngle(angle);
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
    public override void updateDescription() {
        description = prefabObject.GetComponent<Projectile>().description;

        if (character != null) {
            calculateAmt();
            description += " dealing " + valueAmt.getAmtValueFromName(this, "Damage");
        }
    }

    public override void reset() {
        numProjectilesLaunched = 0;
        delaySinceLastProjectile = 0;
        numWavesLaunched = 0;
        //Only the first wave requires the animation to be done to fire. Subsequent waves just play the animation for looks
        executeAbilityOnEvent = true;
    }
    private void FixedUpdate() {
        cooldown();
        if (abilityStarted) {
            delaySinceLastProjectile += Time.fixedDeltaTime;
            delaySinceLastWave += Time.fixedDeltaTime;
        }
    }
}
