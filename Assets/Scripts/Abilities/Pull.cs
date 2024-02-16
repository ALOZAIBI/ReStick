using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Blitzcrank Q, range increases with Inf
public class Pull : Ability
{
    //The arm that will do the pulling
    [SerializeField]private LineRenderer arm;

    //Used to run the doPull function
    bool pulling = false;
    //The arm reached the target
    bool reached = false;
    public override void Start() {
        base.Start();
        updateDescription();

        arm.positionCount = 2;
    }

    public override void doAbility() {
        //Calculate the range of the ability
        calculateAmt();
        rangeAbility = valueAmt.getAmtValueFromName(this,"Range");

        if (available && character.selectTarget(targetStrategy, rangeAbility)) {
            playAnimation("castRaise");
            lockedTarget = character.target;
        }
    }
    
    public override void updateDescription() {
        description = "Pull target ";
    }

    public override void executeAbility() {
        //If the target dies before the ability is executed then try to find another target in range, if there is no in range, then simply cancel the ability
        if (lockedTarget == null || !lockedTarget.alive) {
            if (character.selectTarget(targetStrategy, rangeAbility)) {
                lockedTarget = character.target;
            }
            else
                return;
        }
        //We can now safely pull the target(Keep both points of arm at the same position to start)
        arm.gameObject.SetActive(true);
        arm.SetPosition(0, character.transform.position);
        arm.SetPosition(1, character.transform.position);
        pulling = true;

        startCooldown();
    }

    private void doPull() {
        if (!reached) {
            arm.SetPosition(0, character.transform.position);
            float step = valueAmt.getAmtValueFromName(this, "Speed") * Time.fixedDeltaTime;
            Vector2 newPosOfArm = Vector2.MoveTowards(arm.GetPosition(1), lockedTarget.transform.position, step);
            arm.SetPosition(1, newPosOfArm);
            if(Vector2.Distance(arm.GetPosition(1), lockedTarget.transform.position) < 0.1f)
                reached = true;
        }
        else {
            //If the arm has reached the target, then pull the target to the caster
            lockedTarget.transform.position = Vector2.MoveTowards(lockedTarget.transform.position, character.transform.position, valueAmt.getAmtValueFromName(this, "Speed") * Time.fixedDeltaTime);
            //Keep the arm attached to the target
            arm.SetPosition(1, lockedTarget.transform.position);
            if (Vector2.Distance(lockedTarget.transform.position, character.transform.position) < 0.5f) {
                //The target has been pulled to the caster
                pulling = false;
                reached = false;
                arm.gameObject.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(pulling)
            doPull();

        cooldown();
    }
}
