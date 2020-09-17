using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DeployBehavior Component;
    public GameObject obj;
    private GameObject test;


    public void OnBeginDrag(PointerEventData eventData)
    {
        test = Instantiate(obj, new Vector3(Input.mousePosition.x, Input.mousePosition.y, -0.5f), Quaternion.identity);
        Time.timeScale = 0.3f;
        Debug.Log("드래그 시작");
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        test.GetComponent<DeployBehavior>().locatePolygon(realPos, 0.5f);
        //test.locatePolygon(realPos, 0.5f);
        //test.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 23f));
        Debug.Log("드래그 중");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        test.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 24.5f));
        Time.timeScale = 1f;
        Debug.Log("드래그 종료");
    }
}
