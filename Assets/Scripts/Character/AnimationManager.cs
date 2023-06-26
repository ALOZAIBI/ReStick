using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Character character;
    public Character target;
    //So this is set to true when an animation that we don't want to be interupted is playing. Like for example attack/cast animations.
    public bool midAnimation = false;
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
        Vector3 dest = agent.destination;


        //0 is right 1 is left
        float angle = (transform.position - dest).x < 0 ? 0 : 1;
        animator.SetFloat("IdleAngle", angle);
    }

    public void attack() {
        animator.SetTrigger("attack");
        midAnimation = true;
        target = character.target;
    }
    public void attackEvent() {
        midAnimation = false;
        character.executeAttack(target);
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name + agent.velocity);
    }
}
