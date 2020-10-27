using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckerBehaviour : MonoBehaviour
{
    public GameObject pathTrackerAgent;
    public float trackerDuration;

    private GameObject pathTrackerObject;
    private NavMeshAgent agent;
    private NavMeshPath path;
    private NavMeshPath tempPath;
    private GameObject destination;

    private GameObject m_pathTrackerAgent;
    private float countDown;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        pathTrackerObject = GameObject.Find("PathTracker");
        CalculatePath();
        ApplyPath();
    }

    private void Update()
    {
        if (pathTrackerObject.transform.childCount == 0)
        {
            m_pathTrackerAgent = null;
        }

        if (m_pathTrackerAgent == null)
        {
            if (countDown <= 0f)
            {
                m_pathTrackerAgent = Instantiate(pathTrackerAgent, transform.position, transform.rotation, pathTrackerObject.transform);
                countDown = trackerDuration;
            }
            
            countDown -= Time.deltaTime;
        }
    }

    private void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        m_pathTrackerAgent = null;
        countDown = 0f;
    }

    public bool CalculatePath()
    {
        tempPath = new NavMeshPath();
        destination = GameObject.FindWithTag("Destination");
        agent.CalculatePath(destination.transform.position, tempPath);

        if (tempPath.status == NavMeshPathStatus.PathPartial)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ApplyPath()
    {
        path = tempPath;

        if (pathTrackerObject.transform.childCount != 0)
        {
            Destroy(m_pathTrackerAgent);
            m_pathTrackerAgent = null;
            countDown = 0;
        }
    }
}
