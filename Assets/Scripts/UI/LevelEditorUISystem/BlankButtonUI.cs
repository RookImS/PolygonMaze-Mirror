using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlankButtonUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject blankPrefab; // prefab

    private GameObject currentObject; // current dragged real gameObject
    private GameObject hitGameObject;// wallObject
    private float currentObjectHeight;
    private Vector3 realPos;
    private LayerMask wallLayer;
    private bool isDeployEnable;

    public void Awake()
    {
        currentObject = null;
        hitGameObject = null;
        currentObjectHeight = 2f;
        wallLayer = 1 << LayerMask.NameToLayer("WallCollidor");
        isDeployEnable = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        realPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentObject = GameObject.Instantiate(blankPrefab, new Vector3(realPos.x, currentObjectHeight, realPos.z), Quaternion.identity);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray;
        RaycastHit hit;

        realPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 2f, wallLayer))
        {
            hitGameObject = hit.collider.gameObject;
            OnTheWall(hitGameObject);
        }
        else
        {
            hitGameObject = null;
            NotOnTheWall();
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDeployEnable)
        {
            hitGameObject.SetActive(false);

            if (hitGameObject != null)
                LevelEditor.instance.AddBlank(currentObject, hitGameObject);
            else
                Debug.Log("hitGameObject instance is null");
        }
        else
        {
            Destroy(currentObject);
        }
    }

    public void OnTheWall(GameObject hitGameObject)
    {
        if (hitGameObject.transform.GetChild(0).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentObject, 0);
        }
        else if (hitGameObject.transform.GetChild(1).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentObject, 1);
        }
        else if (hitGameObject.transform.GetChild(2).gameObject.activeSelf == true)
        {
            SetActiveChildGameObject(currentObject, 2);
        }
        else
        {
            Debug.Log("WallComponent is not 0~2 in OnTheWall of BlankButtonUI.cs");
        }

        currentObject.transform.position = new Vector3(hitGameObject.transform.position.x,
            currentObjectHeight, hitGameObject.transform.position.z);
        currentObject.transform.eulerAngles = new Vector3(0f, hitGameObject.transform.rotation.eulerAngles.y % 360, 0f);


        isDeployEnable = true;
    }

    public void NotOnTheWall()
    {
        SetActiveChildGameObject(currentObject, 0);

        currentObject.transform.position = new Vector3(realPos.x, 2f, realPos.z);
        currentObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);

        isDeployEnable = false;
    }

    public void SetActiveChildGameObject(GameObject gobj, int num)
    {
        if(num == 0)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(true);
            gobj.transform.GetChild(1).gameObject.SetActive(false);
            gobj.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if(num == 1)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(false);
            gobj.transform.GetChild(1).gameObject.SetActive(true);
            gobj.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if(num == 2)
        {
            gobj.transform.GetChild(0).gameObject.SetActive(false);
            gobj.transform.GetChild(1).gameObject.SetActive(false);
            gobj.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("num is not 0~2 in SetActiveChildGameObject of BlankButtonUI.cs");
        }

    }
}
