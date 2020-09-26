using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRendererTest : MonoBehaviour
{
    private LineRenderer lineRenderer;

    GameObject obj;

    // Use this for initialization

    void Start()

    {

        //라인렌더러 설정

        lineRenderer = GetComponent<LineRenderer>();

        //lineRenderer.SetColors(Color.red, Color.yellow);

        //lineRenderer.SetWidth(0.1f, 0.1f);

        obj = GameObject.Find("navi");


        //라인렌더러 처음위치 나중위치

        //lineRenderer.SetPosition(0, transform.position);

        //lineRenderer.SetPosition(1, transform.position + new Vector3(0, 10, 0));

        //lineRenderer.SetPosition(2, transform.position + new Vector3(10, 20, 0));



    }



    // Update is called once per frame

    void Update()

    {
        //for(float index = 0.0f; ;)
        //{
        //    lineRenderer.SetPosition((int)index++, obj.transform.position);
        //    if (obj.activeSelf == false)
        //    {
        //        break;
        //    }
        //}
    }
}
