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

    EnemyData m_EnemyData;

    void Awake()
    {
        Init();   
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        MoveToDestination();
    }

    // Update is called once per frame
    void Update()
    {
        //empty
    }
    
    void Init()
    {
        m_EnemyData = GetComponent<EnemyData>();
        m_EnemyData.Init();

        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
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

    public void Damage(int damage)
    {
        m_EnemyData.Damage(damage);
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
        m_EnemyData.Attack();
        Destroy(this.gameObject);
    }

    public void ChangeAgentSpeed(float speed)
    {
        agent.speed = speed;
    }
}
