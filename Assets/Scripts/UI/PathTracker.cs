using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathTracker : MonoBehaviour
{
    public GameObject sprite;
    public float spriteRate;
    public float spriteDuration;

    private GameObject destination;
    private NavMeshAgent agent;
    private bool reachDest;
    private float countDown;
    private GameObject traceObject;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        agent.SetDestination(destination.transform.position);
        traceObject = new GameObject("Trace");
        traceObject.transform.SetParent(this.transform.parent);
    }

    private void Update()
    {
        if (reachDest)
        {
            if (traceObject.transform.childCount == 0)
            {
                Destroy(traceObject);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (countDown <= 0f)
            {
                GameObject trace = Instantiate(sprite, transform.position, transform.rotation, traceObject.transform);
                trace.GetComponent<Trace>().Init(spriteDuration);
                countDown = 1f / spriteRate;
            }
            countDown -= Time.deltaTime;
        }
    }
    private void Init()
    {
        destination = GameObject.FindWithTag("Destination");
        agent = GetComponent<NavMeshAgent>();
        reachDest = false;

        countDown = 1f / spriteRate;
        traceObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            reachDest = true;
        }
    }
}
