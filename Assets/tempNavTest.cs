using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tempNavTest : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject obje;
    public Vector3 test;
    public bool go;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        test = obje.transform.position;
        agent.SetDestination(test);
    }
}
