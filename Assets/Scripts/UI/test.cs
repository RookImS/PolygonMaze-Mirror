using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject targetObject;
    private GameObject newObject;
    private GameObject hitObject;

    Vector3 mousePos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        newObject = Instantiate(targetObject, mousePos, targetObject.transform.rotation);
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;

        hitObject = newObject.GetComponent<DeployBehavior>().LocateTower(mousePos);
        newObject.GetComponent<DeployBehavior>().CheckOverlap();
        //{
        //    Debug.Log("overlap!");
        //}
    }
    public void OnEndDrag(PointerEventData eventData)
    { 
        newObject.GetComponent<DeployBehavior>().DeployTower(hitObject);
    }
}
