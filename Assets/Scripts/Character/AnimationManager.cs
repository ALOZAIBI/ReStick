using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void move(bool movementBool) {
        animator.SetBool("movementBool", movementBool);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name + agent.velocity);
    }
}
