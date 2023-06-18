using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//notebook page 59 for calculations
//this ability took about 2:30 hours to code initially probably more time later on
public class MagicAvalanche : Ability
{
    //Creates N Magic Balls around caster then after delay Throw them Towards Enemy
    //each Ball has a random ish Size that deals Random Ish Damage Depending on AMT
    //N Scales with AMT

    public float channelTime;
    public float currentChannelTime;
    //N
    public int ballAmount;

    public float delayBetweenBall;
    public float currentDelayBetweenBall;

    //this is used to alternate between spawning left side and right side
    //if 1 then right if -1 then left
    public int side=1;

    //used so that on channel start spawn a ball
    //and apply a slow debuff to self
    public bool channelStart = true;

    public GameObject selfSlowDebuf;

    public override void Start() {
        side = 1;
        channelStart = true;

        base.Start();
        updateDescription();
    }
    public override void doAbility() {
        if (available && character.selectTarget(targetStrategy,rangeAbility)) {
            calculateAmt();
            ballAmount = (int)amt;
            delayBetweenBall = channelTime / ballAmount;
            //slow self while casting htis ability
            if (channelStart) {
                Buff buff = Instantiate(selfSlowDebuf).GetComponent<Buff>();
                buff.MS = -1.75f;
                buff.caster = character;
                buff.target = character;
                buff.duration = channelTime+0.75f;
                buff.code = "MAGIC AVALAANCHE";
                buff.applyBuff();
            }
            //while channeling
            if (currentChannelTime < channelTime) {
                
                //Counts up the time
                if(currentDelayBetweenBall < delayBetweenBall && channelStart == false) {
                    currentDelayBetweenBall += Time.fixedDeltaTime;
                }
                //once time achieved
                else {
                    channelStart = false;
                    //Debug.Log("Summoning Ball");
                    //reset time and spawn a ball
                    currentDelayBetweenBall = 0;
                    //gets size and damage values
                    float randomVal = Random.Range(0.2f, 0.5f);
                    //The way 0.33 was calculated is I assumed there will be 6 balls spawning in that case each ball should have a maximum size of 1 which is how we found 0.33
                    //and all in all size is capped at 3 when the dude is very powerful
                    float size = Mathf.Clamp((amt * 0.33f) * randomVal, 0.2f, 3);
                    //The way 13.3 was calcualted again we assumed there will be 6 balls so in that case each ball can deal a maximum of 40 etc...
                    //get youssef's help to create an equation so that total average damage isn't exponential. Currently it's exponential since ball count jumps every 10 MD and at the same time each ball damage increases so yea...
                    float damage = ((amt * 13.33f) * randomVal);
                    //Debug.Log("Damage:" + damage + "Ball Amount" + ballAmount + "Total Average Damage" + amt * 13.33f * 0.35f * ballAmount);

                    Vector2 randomPos;
                    //gets position of where to summon
                    //if it's to the right  the minimum of the X would be to the right of character else to the left of character
                    if (side == 1) {
                        randomPos = (Vector2)character.transform.position + new Vector2(Random.Range(0.5f, side * ballAmount / 4f), Random.Range(-ballAmount / 3,ballAmount / 3));
                    }
                    else {
                        randomPos = (Vector2)character.transform.position + new Vector2(Random.Range(-0.5f, side * ballAmount / 4f), Random.Range(-ballAmount / 3,ballAmount / 3));
                    }
                    //alternates side
                    side *= -1;
                    
                    
                    //creates the projectile
                    GameObject objProjectile = Instantiate(prefabObject, character.transform.position, character.transform.rotation);
                    //sets the size
                    objProjectile.transform.localScale = new Vector3(size, size, size);
                    ProjectileDestinationThenTarget projectile = objProjectile.GetComponent<ProjectileDestinationThenTarget>();
                    //sets the shooter to be the caster of this ability
                    projectile.shooter = character;
                    //sets the damage amount
                    projectile.DMG = damage;
                    //sets the target
                    projectile.target = character.target;
                    projectile.destination = randomPos;
                    projectile.toDestinationSpeed = 4;

                    //speed minimum is 5 and max is 12
                    projectile.speed = Mathf.Clamp(amt * 2, 12, 20);

                    projectile.delayWanted = channelTime - currentChannelTime;
                }
                //counts up chanel time
                currentChannelTime += Time.fixedDeltaTime;
            }
            //when done channeling
            else {
                startCooldown();
                currentChannelTime = 0;
                currentDelayBetweenBall = 0;
                channelStart = true;
            }
        }

    }
    public override void updateDescription() {
        if(character == null) {
            calculateAmt();
            description = "Throws a barrage of magical spheres that deal around" + ((amt * 13.33f) * 0.35f).ToString("F1") + " DMG each";
        }
        else {
            description = "Throws a barrage of magical spheres";
        }
    }

    private void FixedUpdate() {
        cooldown();
    }


}
