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

        hitObject = newObject.GetComponent<DeployBehaviour>().LocateTower(mousePos);
    }
    public void OnEndDrag(PointerEventData eventData)
    { 
        newObject.GetComponent<DeployBehaviour>().DeployTower(hitObject);
    }
}
