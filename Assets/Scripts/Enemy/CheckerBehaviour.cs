using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private GameObject destination; // test code

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        MoveToDestination();
    }

    // Start is called before the first frame update
    void Start()
    {
        //empty
    }

    // Update is called once per frame
    void Update()
    {
        //empty
    }

    public bool MoveToDestination()
    {
        destination = GameObject.FindWithTag("Destination");
        agent.CalculatePath(destination.transform.position, path);

        if (path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log("Invalid path");
            return false;
        }
        else
        {
            Debug.Log("valid path");
            return true;
        }
    }
}
