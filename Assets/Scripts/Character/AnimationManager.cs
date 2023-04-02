using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void move(bool movementBool) {
        animator.SetBool("movementBool", movementBool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
