using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform goal;

    private Vector3 originPosition;

    void Start()
    {
        originPosition = transform.position;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }


    public void Reeeeeest()
    {
        transform.position = originPosition;
    }
}
