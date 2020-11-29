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
    private List<GameObject> traceList;
    private GameObject traceObject;
    private float fadingAmount;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        destination = GameObject.FindWithTag("Destination");

        if (destination != null)
        {
            agent.SetDestination(destination.transform.position);

            traceList = new List<GameObject>();
            traceObject = new GameObject("Trace");
            traceObject.transform.SetParent(this.transform.parent);
        }
        else
            Debug.Log("destination is null");
    }

    private void Update()
    {
        FadeTrace();

        if (reachDest)
        {
            if (traceObject.transform.childCount == 0)
            {
                DestroyTrace();
            }
        }
        else
        {
            if (countDown <= 0f)
            {
                MakeTrace();
                countDown = 1f / spriteRate;
            }
            countDown -= Time.deltaTime;
        }
    }
    private void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        reachDest = false;

        countDown = 0f;
        traceList = null;
        traceObject = null;
    }

    private void MakeTrace()
    {
        GameObject trace = Instantiate(sprite, transform.position, transform.rotation, traceObject.transform);
        traceList.Add(trace);
    }

    private void FadeTrace()
    {
        Renderer rend;
        Color fadingColor;
        int count = traceList.Count;

        fadingAmount = 1f / spriteDuration;
        fadingAmount *= Time.deltaTime;

        for (int i = 0; i < count; i++)
        {
            rend = traceList[i].transform.GetChild(0).GetComponent<Renderer>();
            fadingColor = rend.material.color;

            fadingColor.a -= fadingAmount;
            rend.material.color = fadingColor;

            if (rend.material.color.a <= 0)
            {
                Destroy(traceList[i]);
                traceList.RemoveAt(i);
                i--;
                count--;
            }
        }
    }

    public void DestroyTrace()
    {
        Destroy(traceObject);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            reachDest = true;
        }
    }
}
