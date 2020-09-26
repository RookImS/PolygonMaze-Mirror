using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject targetObject;
    private GameObject deployObject;

    Vector3 mousePos;
    Vector3 realPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        deployObject = Instantiate(targetObject, mousePos, targetObject.transform.rotation);
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        realPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.5f));
        //realPos = Camera.main.ScreenToWorldPoint(mousePos);

        deployObject.GetComponent<DeployBehavior>().LocateTower(realPos);

        deployObject.GetComponent<DeployBehavior>().CheckOverlap();
        //{
        //    Debug.Log("overlap!");
        //}
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        deployObject.GetComponent<DeployBehavior>().DeployTower(deployObject.transform.position);
    }
}
