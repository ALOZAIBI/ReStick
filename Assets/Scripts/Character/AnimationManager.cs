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
        Vector3 dest = agent.destination;
        //look at unit circle
        float angle = Mathf.Atan2(dest.y - transform.position.y, dest.x - transform.position.x)*Mathf.Rad2Deg;
        //cuz angle goes to negatives for some reason
        if(angle < 0) {
            angle += 360;
        }
        //float angle = Vector3.Angle(transform.position, dest);
        //if (name == "Loasp") {
        //    Debug.Log(dest);
        //    Debug.Log(angle);
        //}
        animator.SetFloat("MoveAngle",angle);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(name + agent.velocity);
    }
}
