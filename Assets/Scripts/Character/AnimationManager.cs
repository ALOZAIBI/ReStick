using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour {
    //Attacking has 2 transition states back to walk/idle one is if interrupt is true which cuts the animation as soon as the function is executed,
    //the other is if the animation is finished.

    //Notice how when an animation starts it is not interruptible however once it executes the event it is.
    //--------------------------------------------------------------------------------------------
    //-----------------FOR NOW IGNORE INTERRUPT SYSTEM MIGHT NOT EVEN BE IMPORTANT-----------------
    //-----------------HOWEVER I AM STILL USING INTERRUPTIBLE TO CHECK IF I CAN PLAY THE ANIMATION NOW-----------------
    //-------------------ITS JUST THAT IT DOESN'T ACTUALLY USE THE INTERRUPT PARAMTER IN THE ANIMATION-----------------`
    //--------------------------------------------------------------------------------------------
    public Animator animator;
    public NavMeshAgent agent;
    public Character character;
    public Character target;
    //So this is set to true when an animation that we don't want to be interupted is playing. Like for example attack/cast animations.
    public bool interruptible = false;
    public bool attackBuffered;

    //If true then the attack is ranged
    private bool attackRanged = false;

    public Ability abilityBuffer;
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void move(bool movementBool) {
        animator.SetBool("movementBool", movementBool);
        //if movement is set to false run the idle animation and stop the movement animation.
        if (!movementBool) {
            idle();
            return;
        }
        Vector3 dest = agent.destination;
        //0 is right 1 is left
        float angle = (transform.position - dest).x < 0 ? 0 : 1;

        animator.SetFloat("MoveAngle", angle);
    }
    //This is used inDashAlot, when all dashes are done the ability becomes interruptible
    public void forceStop() {
        //using animator.SetTrigger causes a bug for DashALotThenSheath since this trigger and another trigger(ATTACK) will be set at the same time which will make the animator ignore one of them, so don't do that mistake and dont use triggers for this.
        //animator.SetTrigger("forceIdle");
        abilityBuffer = null;
        //attackBuffered = false;
        interruptible = true;
        character.currentDashingAbility = null;
        //Debug.Log("Interruptible:"+interruptible);
    }
    //if you want to stop the animation but continue within the same ability, such as the case in dashAlot
    public void keepAbilityMakeInterupttible() {
        interruptible = true;
    }
    private void idle() {
        //idling facing that direction
        setTargetAngle();
    }
    private void setTargetAngle() {
        if(character.target!=null)
            animator.SetFloat("TargetAngle", (transform.position - character.target.transform.position).x < 0 ? 0 : 1);
    }

    public void startAttackCooldown() {
        character.startAttackCooldown();
    }
    public void attack(bool ranged) {
        ////if there's an animation buffered(ability or attack) then interrupt
        //if (interruptible)
        //    animator.SetTrigger("interrupt");
        //If there is no ability buffered and the animator is interupttible 
        if (abilityBuffer==null && interruptible) {
            //Debug.Log("Attack interruptible before setting:" + interruptible);
        setTargetAngle();
        animator.SetTrigger("attack");
        interruptible = false;
            //Debug.Log("Attack set interruptible to:" + interruptible);
        target = character.target;
        animator.SetFloat("animationSpeed", 1 + character.AS*0.5f);

        attackRanged = ranged;
        }
    }
    public void attackEvent() {
        interruptible = true;
        if(attackRanged)
            character.executeAttackRanged(target);
        else
            character.executeAttackMelee(target);
    }

    //This is how the abilities work
    //If toBeCast==null >>>> animtion.cast>after some animation> animation.castEvent()>>> executeAbility()
    //Casts the ability with the raise animation
    public void cast(Ability ability,string animation) {
        //If there is no ability buffered or the same ability thaqt is already buffered called this. and the animator is interupttible
        if (interruptible && (abilityBuffer == null || abilityBuffer == ability)) {
            setTargetAngle();
            abilityBuffer = ability;
            switch (animation) {
                case "castRaise":
                    animator.SetTrigger("castRaise");
                    break;
                case "castAoePush":
                    animator.SetTrigger("castAoePush");
                    break;
                case "castPierce":
                    animator.SetTrigger("castPierce");
                    break;
                case "castDash":
                    animator.SetTrigger("castDash");
                    break;
                default:
                    Debug.LogError("Animation not found");
                    break;
            }
            animator.SetFloat("animationSpeed", 0.75f + character.AS * 0.1f + character.CDR * 0.9f);
            interruptible = false;
            //Debug.Log("Cast set interruptible to :" + interruptible);
        }
    }


    public void castEvent() {
        interruptible = true;
        character.selectTarget(abilityBuffer.targetStrategy, abilityBuffer.rangeAbility,abilityBuffer.excludeTargets());
        if(abilityBuffer.executeAbilityOnEvent)
            abilityBuffer.executeAbility();
        abilityBuffer = null;
    }
    //In some cases we don't want it to become interupttible after the castEvent is done such as the case of dashalothtenseheath, since we don't wnat it to be interruptible until after All the dashes are done. We should however make it interuptible manually using forceStop()
    public void castEventDontInterrupt() {
        Debug.Log("CasteventDontInterrupt set interruptible to :" + interruptible);
        character.selectTarget(abilityBuffer.targetStrategy, abilityBuffer.rangeAbility);
        if(abilityBuffer.executeAbilityOnEvent)
            abilityBuffer.executeAbility();
    }
    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if(name == "Mathew") {
    //        Debug.Log("Update:" + interruptible);
    //    }
    //}
}
