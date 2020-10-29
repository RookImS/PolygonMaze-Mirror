using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckerBehaviour : MonoBehaviour
{
    public GameObject pathTrackerAgent;
    public float trackerDuration;

    Vector3[] c;
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
        path = tempPath;
        c = path.corners;   // 왠지 몰라도 한번 넣어줘야 오류가 안생김??..
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

    private bool checkPathChange(Vector3[] c1, Vector3[] c2)
    {
        if (c1.Length != c2.Length)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < c1.Length; i++)
            {
                if (c1[i].x != c2[i].x
                   || c1[i].y != c2[i].y
                   || c1[i].z != c2[i].z)
                    return true;
            }
        }
        return false;
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
        if (pathTrackerObject.transform.childCount != 0)
        {
            if (checkPathChange(path.corners, tempPath.corners))
            {
                Debug.Log("Change");
                m_pathTrackerAgent.GetComponent<PathTracker>().DestroyTrace();
                m_pathTrackerAgent = null;
                countDown = 0f;
            }
        }

        path = tempPath;
    }
}