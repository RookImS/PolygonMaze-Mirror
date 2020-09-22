using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployBehavior : MonoBehaviour
{
    public GameObject Tower;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);

        //if (Input.GetMouseButtonDown(0))
        //{

        //}
        //if(Input.GetMouseButton(0))
        //{
        //    locatePolygon(realPos, 0.5f);
        //}
        //if(Input.GetMouseButtonUp(0))
        //{
            
        //}
        
    }
    
    public void locatePolygon(Vector3 pos, float distance)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        transform.position = new Vector3(pos.x, 0.75f, pos.z);

        if (Physics.Raycast(ray, out hit))
        {
            Collider objectHit = hit.collider;
            transform.eulerAngles = new Vector3(0f, objectHit.transform.rotation.eulerAngles.y, 0f);
            Vector3 direction = transform.rotation * Vector3.forward;
            Debug.Log(direction.x + ", " + direction.y + ", " + direction.z);
            Vector3 targetPoint = direction * distance;
            transform.position = objectHit.transform.position + targetPoint;
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

}
