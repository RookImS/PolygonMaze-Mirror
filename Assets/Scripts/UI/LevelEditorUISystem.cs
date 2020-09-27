using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorUISystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject targetObject;
    private GameObject deployObject;
    private Vector3 mousePos;
    private Vector3 realPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        realPos = Camera.main.ScreenToWorldPoint(mousePos);
        deployObject = Instantiate(targetObject, 
            new Vector3(realPos.x, 1.5f, realPos.z), targetObject.transform.rotation);
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;
        realPos = Camera.main.ScreenToWorldPoint(mousePos);
        deployObject.transform.position = new Vector3(realPos.x, 1.5f, realPos.z);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //deployObject.GetComponent<DeployBehavior>().DeployTower(deployObject.transform.position);
    }
}

