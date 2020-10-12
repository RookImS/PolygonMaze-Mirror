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
        agent.SetDestination(destination.transform.position);
    }

    void Init()
    {
        m_EnemyData = GetComponent<EnemyData>();
        m_EnemyData.Init();

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.transform.position);
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
