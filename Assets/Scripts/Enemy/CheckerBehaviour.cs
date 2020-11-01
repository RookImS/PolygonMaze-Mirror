using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CheckerBehaviour : MonoBehaviour
{
    public GameObject pathTrackerAgent;
    public float trackerDuration;
     
    private Vector3 basePosition;
    private Vector3[] c;    // trash value for error correct
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
        basePosition.x = Mathf.Round(transform.position.x * 10f) / 10f;
        basePosition.y = Mathf.Round(transform.position.y * 10f) / 10f;
        basePosition.z = Mathf.Round(transform.position.z * 10f) / 10f;

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

    private bool CheckPathChange(Vector3[] c1, Vector3[] c2)
    {
        if (c1.Length != c2.Length)
        {
            Debug.Log("check");
            return true;
        }
        else
        {
            for (int i = 0; i < c1.Length; i++)
            {
                if (Mathf.Round(c1[i].x * 10f) / 10f != Mathf.Round(c2[i].x * 10f) / 10f
                   || Mathf.Round(c1[i].y * 10f) / 10f != Mathf.Round(c2[i].y * 10f) / 10f
                   || Mathf.Round(c1[i].z * 10f) / 10f != Mathf.Round(c2[i].z * 10f) / 10f)
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
            if (CheckPathChange(path.corners, tempPath.corners))
            {
                Debug.Log("Change");
                m_pathTrackerAgent.GetComponent<PathTracker>().DestroyTrace();
                m_pathTrackerAgent = null;
                countDown = 0f;
            }
        }

        path = tempPath;
    }

    public void FixPosition()
    {
        if (Mathf.Round(transform.position.x * 10f) / 10f != basePosition.x
           || Mathf.Round(transform.position.y * 10f) / 10f != basePosition.y
           || Mathf.Round(transform.position.z * 10f) / 10f != basePosition.z)
            agent.Warp(basePosition);
    }
}