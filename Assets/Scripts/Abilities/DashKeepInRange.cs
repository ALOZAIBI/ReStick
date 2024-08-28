using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This ability required way more math/thinking than expected.
//Anyways this is the idea of how it will be implemented.
// Select Random Enemy e within ability range
// Select random point on circle of radius caster's range with center as e
// Dash to that point
// If there are enemies around (within range of caster) then keep dashing in that direction(using angle)
public class DashKeepInRange : Ability
{
    [SerializeField]private Vector2 pointToDashTo;
    [SerializeField] private float angle;
    private Vector2 direction;
    private LayerMask mask;

    //To make sure that the pointToDashTo is on the ground
    private LayerMask ground;
    //If 0.9 then it is okay when we are at 90% of our range

    private float rangeLeeWayPercent = 0.9f;

    private bool pointSet = false;
    private bool rangeIsSet = false;
    public override void Start() {
        base.Start();
        updateDescription();
        mask = LayerMask.GetMask("Characters");
    }

    private void FixedUpdate() {
        cooldown();
    }

    public override bool doAbility() {
        bool done = false;
        float maxRange = character.Range + valueAmt.getAmtValueFromName(this, "Range");
        calculateAmt();

        //The range will start low then will slowly increase(It will prioritize closer enemies)
        if (available && canUseDash()) {
            rangeAbility = maxRange / 5;
            for(int i = 1; i < 5; i++) {
                rangeAbility *= i;
                //If there is a character within that range
                if(character.selectTarget(targetStrategy, rangeAbility)) {
                    rangeIsSet = true;
                    break;
                }
            }

            //Return false (not done)
            if (!rangeIsSet) {
                return done;
            }
        }
        //If we found a target and set the range
        if (rangeIsSet) {

            if (!pointSet) {
                startAbilityActivation();
                //Gets a random enemy within the ability's range
                Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, rangeAbility,mask);
                List<Character> enemies = new List<Character>();
                foreach(Collider2D c in colliders) {
                    Character temp = c.GetComponent<Character>();
                    if (temp.team != character.team) {
                        enemies.Add(temp);
                    }
                }
                if (enemies.Count == 0) {
                    return false;
                }
                //Select a random enemy
                Character enemy = enemies[Random.Range(0, enemies.Count)];

                //Generate random angle (to find point on radius) in radians
                float angleForPointOnRadiusRD = Random.Range(0, 360) * Mathf.Deg2Rad;

                //Using trigonmetry (Cah) to find the x of the point on the radius we at this point have the angle and the hypotenuse(character range)

                float xP = Mathf.Cos(angleForPointOnRadiusRD) * (character.Range*rangeLeeWayPercent);

                //Using trignometry (Toa) to find the y of the point on the radius

                float yP = Mathf.Tan(angleForPointOnRadiusRD) * xP;

                pointToDashTo = new Vector2(enemy.transform.position.x + xP, enemy.transform.position.y + yP);

                direction = pointToDashTo - (Vector2)character.transform.position;
                direction.Normalize();  

                character.invulnerable++;

                pointSet = true;

            }

            playAnimation("castDash");
            done = true;

        }
        //This is done since the dash animation does castEventDoNotInterrupt which prevents the above playAnimation from triggering. So we are triggering it here once the playAnimation has been triggered atleasst once
        //It is done outside the initial if statement so that we can go through the if Dead or null check. Becaise if there are no characters within range then the ability won't be executed and won't go through the check.
        if (character.CurrentDashingAbility == this) {
            executeAbility();
            done = true;
        }
        return done;
    }

    //So now in step 0 we will be dashing  to the pointtoDashTo
    //Then in step 1 we check if there are any enemies within range of the caster if yes we will keep dashing in the direction of the angle

    public override void executeAbility() {

        character.CurrentDashingAbility = this;
        character.agent.enabled = false;


        if (step == 0) {
            character.transform.position = Vector2.MoveTowards(character.transform.position, (Vector3)pointToDashTo, valueAmt.getAmtValueFromName(this, "Speed") * Time.fixedDeltaTime);

            //Once we are close enough to the point we will move to the next step
            if (Vector2.Distance(character.transform.position, pointToDashTo) < 0.1f) {
                step++;
            }
        }

        if(step == 1) {
            //Check if there are enemies inside the range of the caster 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, character.Range*rangeLeeWayPercent, mask);
            bool continueDashing = false;
            foreach (Collider2D c in colliders) {
                Character temp = c.GetComponent<Character>();
                if (temp.team != character.team) {
                    continueDashing = true;
                    break;
                }
            }
            //Continue moving at the angle
            if (continueDashing) {


                character.transform.position = Vector2.MoveTowards(character.transform.position, pointToDashTo + direction*100f, valueAmt.getAmtValueFromName(this, "Speed") * Time.fixedDeltaTime);

            }
            else {
                //If there are no enemies in range then we stop
                step++;
            
            }
        }

        //If we have reached the end of the ability
        if (step == 2) {
            startCooldown();
            character.animationManager.forceStop();
            character.agent.enabled = true;

            //Reset auto attack timer
            character.AtkNext = 0;

            character.invulnerable--;
        }

    }
    public override void updateDescription() {
        description = "Dash tactically, invulnerable while dashing";
    }

    public override void reset() {
        pointSet = false;
        rangeIsSet = false;
        angle = 0;
        pointToDashTo = Vector2.zero;
        step = 0;
    }

}
