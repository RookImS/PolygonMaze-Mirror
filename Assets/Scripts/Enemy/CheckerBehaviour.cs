using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private NavMeshPath tempPath;
    private GameObject destination; // test code

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        CalculatePath();
        ApplyPath();
    }

    public bool CalculatePath()
    {
        tempPath = new NavMeshPath();
        destination = GameObject.FindWithTag("Destination");
        agent.CalculatePath(destination.transform.position, tempPath);

        if (tempPath.status == NavMeshPathStatus.PathPartial)
        {
            //Debug.Log("Invalid path");
            return false;
        }
        else
        {
            //Debug.Log("valid path");
            return true;
        }
    }

    public void ApplyPath()
    {
        path = tempPath;
    }
}
