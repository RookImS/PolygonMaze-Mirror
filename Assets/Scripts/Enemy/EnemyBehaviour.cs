using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshPath path;
    private GameObject destination; // test code

    public EnemyData enemyData => m_EnemyData;
    EnemyData m_EnemyData;

    void Awake()
    {
        m_EnemyData = GetComponent<EnemyData>();
        m_EnemyData.Init();

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

    public void MoveToDestination()
    {
        destination = GameObject.FindWithTag("Destination");
        agent.CalculatePath(destination.transform.position, path);
        ChangeAgentSpeed(m_EnemyData.Stats.stats.speed);

        if (path.status == NavMeshPathStatus.PathPartial)
        {
            //Debug.Log("Invalid path");
        }
        else
        {
            agent.SetPath(path);
            //Debug.Log("valid path");
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            ExitDestination();
        }
        
    }

    public void ExitDestination()
    {
        enemyData.Attack();
        enemyData.Death();
    }

    public void ChangeAgentSpeed(float speed)
    {
        agent.speed = speed;
    }
}
