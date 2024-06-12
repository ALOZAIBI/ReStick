using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Difference between this and LaunchProjectiles is that this one homes in on the target and hence the targets are randomly selected within range
public class LaunchHomingProjectiles : Ability
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

    public bool abilityStarted = false;


    public float numProjectiles = 5;

    public float projectilesFired = 0;
    //Delay between each projectile
    public float delayBetweenProjectiles = 0.1f;

    //Set to a high value so that the first projectile can be fired
    public float timeSinceLastProjectile = 100;

    //to detect only character collisions
    public LayerMask mask;

    public override void Start() {
        base.Start();
        updateDescription();
        mask = LayerMask.GetMask("Characters");
    }



    public override bool doAbility() {
        Debug.Log("Doing ability");
        bool done = false;
        //If I didn't fire everything and it is time to fire
        if(available && projectilesFired < numProjectiles && timeSinceLastProjectile>= delayBetweenProjectiles) {
            Debug.Log("Time to fire");
            //If there is a target within range
            if (character.selectTarget(targetStrategy, rangeAbility)) {
                Debug.Log("Character within range");
                //List of enemies within range
                List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(character.transform.position, (rangeAbility), mask));

                //Exclude allies
                for (int i = 0; i < colliders.Count; i++) {
                    if (colliders[i].GetComponent<Character>().team == character.team) {
                        colliders.RemoveAt(i);
                        i--;
                    }
                }



                //Selects a random target
                int randomIndex = Random.Range(0, colliders.Count);
                Character target = colliders[randomIndex].GetComponent<Character>();

                //Creates a projectile targetting the target
                createProjectile(target);

                done = true;
                

                //If this is the first time the ability is being used play the animation
                if (!abilityStarted) {
                    playAnimation("castRaise");
                    abilityStarted = true;
                    //Just to trigger startAbilityActivation()
                    executeAbility();
                }

                projectilesFired++;
                timeSinceLastProjectile = 0;
            }
        }
        //If all projectiles are fired start cd
        if (projectilesFired >= numProjectiles) {
            startCooldown();
        }
        return done;
    }

    private void FixedUpdate() {
        cooldown();
        //If ability started
        if (abilityStarted) {
            timeSinceLastProjectile += Time.fixedDeltaTime;
        }
    }
    public override void executeAbility() {
        base.executeAbility();
    }

    private void createProjectile(Character target) {

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
        projectile.target = target;
        projectile.setAngle();
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

    public override void reset() {
        abilityStarted = false;
        projectilesFired = 0;
        timeSinceLastProjectile = 100;
    }
    public override void updateDescription() {
        description = "Fires " + numProjectiles + " projectiles at random enemies within range";

        if (character != null) {
            calculateAmt();
            description = "Fires " + numProjectiles + " projectiles at random enemies within range dealing " + valueAmt.getAmtValueFromName(this, "Damage") + " damage each.";
        }
    }
}
