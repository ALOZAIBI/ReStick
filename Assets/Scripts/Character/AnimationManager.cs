using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
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

    public Ability abilityBuffer;
    // Start is called before the first frame update
    void Start()
    {
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
        float angle = (transform.position-dest).x<0 ? 0:1;

        animator.SetFloat("MoveAngle",angle);
    }
    private void idle() {
        //idling facing that direction
        setTargetAngle();
    }
    private void setTargetAngle() {
        if(character.target!=null)
            animator.SetFloat("TargetAngle", (transform.position - character.target.transform.position).x < 0 ? 0 : 1);
    }
    public void attack() {
        ////if there's an animation buffered(ability or attack) then interrupt
        //if (interruptible)
        //    animator.SetTrigger("interrupt");
        if (interruptible) { 
        setTargetAngle();
        animator.SetTrigger("attack");
        interruptible = false;
        target = character.target;
        animator.SetFloat("animationSpeed", 0.75f + character.AS*0.5f);
        }
    }
    public void attackEvent() {
        interruptible = true;
        character.executeAttackMelee(target);
    }
    //This is how the abilities work
    //If toBeCast==null >>>> animtion.cast>after some animation> animation.castEvent()>>> executeAbility()
    //Casts the ability with the raise animation
    public void cast(Ability ability,string animation) {
        //if (interruptible)
        //    animator.SetTrigger("interrupt");
        if (interruptible) {
            setTargetAngle();
            abilityBuffer = ability;
            switch (animation) {
                case "castRaise":
                    animator.SetTrigger("castRaise");
                    break;
                case "castAoePush":
                    animator.SetTrigger("castAoePush");
                    break;
                default:
                    Debug.LogError("Animation not found");
                    break;
            }
            interruptible = false;
        }
    }


    public void castEvent() {
        interruptible = true;
        character.selectTarget(abilityBuffer.targetStrategy, abilityBuffer.rangeAbility);
        abilityBuffer.executeAbility();
        abilityBuffer = null;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name + agent.velocity);
    }
}
