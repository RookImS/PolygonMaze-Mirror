using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DeployBehaviour : MonoBehaviour
{
    public GameObject realTower;
    CheckerBehaviour checker;
    public LayerMask layerMask;
    public NavMeshSurface checkerNav;
    public NavMeshSurface enemyNav;

    public float innerRadius;
    private bool isDeployEnable;
    public bool isOverlapped;

    private void Awake()
    {
        // connect navMesh
        checkerNav = GameObject.Find("CheckerNavMeshSurface").GetComponent<NavMeshSurface>();
        enemyNav = GameObject.Find("EnemyNavMeshSurface").GetComponent<NavMeshSurface>();
        checker = GameObject.FindGameObjectWithTag("Spawner").GetComponent<CheckerBehaviour>();
    }

    public GameObject LocateTower(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // locate tower at mousePos(realPos)
        transform.position = new Vector3(realPos.x, 0.25f, realPos.z);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f, layerMask))
        {
            Collider objectHit = hit.collider;

            // locate proper position
            Vector3 rotateAngle = new Vector3(0f, (objectHit.transform.rotation.eulerAngles.y + 180) % 360, 0f);
            transform.eulerAngles = rotateAngle;

            Vector3 direction = transform.rotation * Vector3.back;
            Vector3 targetPoint = direction * ((innerRadius) - 0.1f);
            transform.position = objectHit.transform.position + targetPoint;


            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = false;
            StartCoroutine(UpdateNavMesh(checkerNav));

            // check path
            if (checker.CalculatePath())
            {
                isDeployEnable = true;
                return objectHit.gameObject.transform.parent.gameObject;
            }
            else
            {
                isDeployEnable = false;
                return null;
            }
        }
        else
        {

            // rotate tower to origin angle
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            isDeployEnable = false;

            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = true;
            StartCoroutine(UpdateNavMesh(checkerNav));

            return null;
        }

 
    }

    public bool CheckPath()
    {
        return checker.CalculatePath(); ;
    }
    public void DeployTower(GameObject neighborObject)
    {

        if (isDeployEnable)
        {
            GameObject newTower = Instantiate(realTower, transform.position, transform.rotation);

            if (!neighborObject.CompareTag("Neutral"))
                neighborObject.GetComponent<TowerBehaviour>().setNeighbor(newTower);
            newTower.GetComponent<TowerBehaviour>().setNeighbor(neighborObject);

            // update enemy navMesh
            StartCoroutine(UpdateNavMesh(enemyNav));
            checker.GetComponent<CheckerBehaviour>().ApplyPath();
            /*
            if (PlayerControl.Instance.UseCost(realTower.GetComponent<TowerData>().cost))
                Instantiate(realTower, pos, transform.rotation);
            
            else
            {
                 부족하다고 알리는 트리거
            }
            */
        }

        // update checker navMesh
        GetComponent<NavMeshModifier>().ignoreFromBuild = true;
        StartCoroutine(UpdateNavMesh(checkerNav));

        Destroy(this.gameObject);
    }

    IEnumerator UpdateNavMesh(NavMeshSurface nm)
    {
        yield return nm.UpdateNavMesh(nm.navMeshData);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("WallCornerSide1") || other.gameObject.CompareTag("WallCornerSide2") || other.gameObject.CompareTag("Spawner") || other.gameObject.CompareTag("Destination"))
        {
            isOverlapped = true;
            isDeployEnable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral") || other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("WallCornerSide1") || other.gameObject.CompareTag("WallCornerSide2") || other.gameObject.CompareTag("Spawner") || other.gameObject.CompareTag("Destination"))
        {
            isOverlapped = false;
        }
    }

}