using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ObstacleCreateButtonUISystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject deployObstacle;
    public GameObject realObstacle;

    private GameObject newObject;
    [HideInInspector] public GameObject hitObject;

    public bool isProgressDeploy;

    Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        isProgressDeploy = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        newObject = null;
        hitObject = null;

        isProgressDeploy = true;
        mousePos = Input.mousePosition;

        newObject = Instantiate(deployObstacle, mousePos, deployObstacle.transform.rotation);

        Time.timeScale = 0.3f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;

        hitObject = newObject.GetComponent<ObstacleDeployBehaviour>().LocateObstacle(mousePos);
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        newObject.GetComponent<ObstacleDeployBehaviour>().DeployObstacle(realObstacle);
        Time.timeScale = 1f;

        isProgressDeploy = false;
    }

    public void LoadObstacle(Vector3 position, Quaternion rotation)
    {
        newObject = Instantiate(deployObstacle, position, rotation);
        newObject.GetComponent<ObstacleDeployBehaviour>().SetIsDeployEnable(true);
        newObject.GetComponent<ObstacleDeployBehaviour>().DeployObstacle(realObstacle);
    }
}