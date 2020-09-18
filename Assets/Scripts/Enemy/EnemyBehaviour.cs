using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private GameObject destination;

    public EnemyData enemyData;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        destination = GameObject.FindWithTag("Destination");
        agent.CalculatePath(destination.transform.position, path);

        if (path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log("Invalid path");
        }
        else
        {
            agent.SetPath(path);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //empty
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            ExitDestination();
        }
        
    }

    public void MoveToDestination()
    {

    }

    public void ExitDestination()
    {
        Destroy(this.gameObject);
        //Enemy decrease life of player
    }
}
