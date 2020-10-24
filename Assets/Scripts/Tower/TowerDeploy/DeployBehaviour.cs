using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DeployBehaviour : MonoBehaviour
{
    public LayerMask layerMask;

    private GameObject towersGameObject;
        private CheckerBehaviour checker;
    private NavMeshSurface checkerNav;
    private NavMeshSurface enemyNav;

    public int cost;
    public float innerRadius;
    public Renderer renderer;

    private bool isDeployEnable;
    private bool isProperLocate;
    private bool isSkipFrame;
    private bool isPathEnable;
    private bool isOverlapped;

    private void Awake()
    {
        // connect navMesh
        checkerNav = GameObject.Find("CheckerNavMeshSurface").GetComponent<NavMeshSurface>();
        enemyNav = GameObject.Find("EnemyNavMeshSurface").GetComponent<NavMeshSurface>();
        checker = GameObject.FindGameObjectWithTag("Checker").GetComponent<CheckerBehaviour>();
        towersGameObject = GameObject.Find("Towers");

        isDeployEnable = false;
        isProperLocate = false;
        isSkipFrame = true;
    }

    private void Update()
    {
        isPathEnable = CheckPath();
        isDeployEnable = CheckDeployEnable();
        isSkipFrame = CheckSkipFrame();
    }

    public GameObject LocateTower(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // locate tower at mousePos(realPos)
        transform.position = new Vector3(realPos.x, 0.25f, realPos.z);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f, layerMask))
        {
            Collider objectHit = hit.collider;

            //locate proper position
            locateProperPos(objectHit.transform);

            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = false;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);

            isProperLocate = true;

            return objectHit.gameObject.GetComponent<SideColliderBehavior>().parentObject;
        }
        else
        {
            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = true;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);

            isProperLocate = false;

            return null;
        }


    }

    private void locateProperPos(Transform pos)
    {
        Vector3 rotateAngle = new Vector3(0f, (pos.rotation.eulerAngles.y + 180) % 360, 0f);
        transform.eulerAngles = rotateAngle;

        Vector3 direction = transform.rotation * Vector3.back;
        Vector3 targetPoint = direction * ((innerRadius) - 0.1f);
        transform.position = pos.position + targetPoint;
    }

    private bool CheckPath()
    {
        bool temp;

        if (isProperLocate)
        {
            temp = checker.CalculatePath();
            if (isSkipFrame)
            {
                return false;
            }
            else
                return temp;
        }

        return false;
    }

    private bool CheckDeployEnable()
    {
        if (PlayerControl.Instance.CheckCost(cost))
        {
            if (isPathEnable && isProperLocate && !isOverlapped)
            {
                renderer.material.color = new Color(1f, 1f, 1f, 85 / 255f);

                return true;
            }
            else
            {
                if (isProperLocate)
                {
                    if (!isSkipFrame)
                    {
                        renderer.material.color = new Color(1f, 170 / 255f, 170 / 255f, 85 / 255f);
                    }
                }
                else
                {
                    renderer.material.color = new Color(1f, 1f, 1f, 85 / 255f);
                }

                return false;
            }
        }
        else
        {
            renderer.material.color = new Color(1f, 170 / 255f, 170 / 255f, 85 / 255f);
            return false;
        }
    }

    private bool CheckSkipFrame()
    {
        if (isProperLocate)
            return false;
        else
            return true;
    }

    public void DeployTower(GameObject neighborObject, GameObject realTower)
    {
        if (isDeployEnable)
        {
            PlayerControl.Instance.UseCost(cost);

            GameObject newTower = Instantiate(realTower, transform.position, transform.rotation, towersGameObject.transform);

            if (!neighborObject.CompareTag("Obstacle"))
                neighborObject.GetComponent<TowerBehaviour>().SetNeighbor(newTower);
            newTower.GetComponent<TowerBehaviour>().SetNeighbor(neighborObject);

            // update enemy navMesh
            enemyNav.UpdateNavMesh(enemyNav.navMeshData);
            checker.ApplyPath();

        }

        // update checker navMesh
        GetComponent<NavMeshModifier>().ignoreFromBuild = true;
        checkerNav.UpdateNavMesh(checkerNav.navMeshData);

        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (TagManager.Instance.isNotDeployableTag(other.gameObject.tag))
        {
            if (isProperLocate)
                isOverlapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagManager.Instance.isNotDeployableTag(other.gameObject.tag))
        {
            isOverlapped = false;
        }
    }

}