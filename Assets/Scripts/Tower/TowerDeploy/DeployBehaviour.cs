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
    public int cost;
    public float innerRadius;
    public Renderer towerRend;
    public Renderer rangeRend;
    public TowerData data;

    private GameObject towersGameObject;
    private Collider preHitCollider;
    private Collider hitCollider;
    private CheckerBehaviour checker;
    private NavMeshSurface checkerNav;
    private NavMeshSurface enemyNav;

    private Ray[] rayArray;
    private float checkRadius1;
    private float checkRadius2;
    private float correction;

    private bool isDeployEnable;
    private bool isProperLocate;
    private bool isPathEnable;
    private bool isOverlapped;
    private bool isSkipFrame_path;
    private bool isSkipFrame_locate;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        transform.Find("Range").localScale = new Vector3(data.Stats.baseStats.recogRange * 2, data.Stats.baseStats.recogRange * 2, 1);
    }
    private void Update()
    {
        isPathEnable = CheckPath();
        isDeployEnable = CheckDeployEnable();
        isSkipFrame_path = CheckSkipFrame();
    }

    private void Init()
    {
        checkerNav = GameObject.Find("CheckerNavMeshSurface").GetComponent<NavMeshSurface>();
        enemyNav = GameObject.Find("EnemyNavMeshSurface").GetComponent<NavMeshSurface>();
        checker = GameObject.FindGameObjectWithTag("Checker").GetComponent<CheckerBehaviour>();
        towersGameObject = GameObject.Find("Towers");
        preHitCollider = null;
        
        isDeployEnable = false;
        isProperLocate = false;
        isSkipFrame_path = true;
        isSkipFrame_locate = true;

        rayArray = new Ray[17];
        checkRadius1 = 0.3f;
        checkRadius2 = 0.5f;
        correction = 1f;
    }

    private bool CheckPath()
    {
        bool temp;

        if (isProperLocate)
        {
            temp = checker.CalculatePath();
            if (isSkipFrame_path)
            {
                return false;
            }
            else
                return temp;
        }

        return false;
    }

    private bool CheckSkipFrame()
    {
        if (isProperLocate)
            return false;
        else
            return true;
    }

    private bool CheckDeployEnable()
    {
        if (PlayerControl.Instance.CheckCost(cost))
        {
            if (isSkipFrame_locate)
            {
                isSkipFrame_locate = false;
                return false;
            }

            if (isPathEnable && isProperLocate && !isOverlapped)
            {
                towerRend.material.color = new Color(1f, 1f, 1f, 225 / 255f);
                rangeRend.material.color = new Color(140 / 255f, 1f, 180 / 255f, 200 / 255f);

                return true;
            }
            else
            {
                if (isProperLocate)
                {
                    if (!isSkipFrame_path)
                    {
                        towerRend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 225 / 255f);
                        rangeRend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 200 / 255f);
                    }
                }
                else
                {
                    towerRend.material.color = new Color(1f, 1f, 1f, 225 / 255f);
                    rangeRend.material.color = new Color(1f, 1f, 1f, 200 / 255f);
                }

                return false;
            }
        }
        else
        {
            towerRend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 225 / 255f);
            rangeRend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 200 / 255f);
            return false;
        }
    }

    private void LocateProperPos(Transform pos)
    {
        Vector3 rotateAngle = new Vector3(0f, (pos.rotation.eulerAngles.y + 180) % 360, 0f);
        transform.eulerAngles = rotateAngle;

        Vector3 direction = transform.rotation * Vector3.back;
        Vector3 targetPoint = direction * ((innerRadius) - 0.1f);
        transform.position = pos.position + targetPoint;
    }

    private class CheckHitCollider
    {
        public CheckHitCollider(Collider collider, int num)
        {
            this.collider = collider;
            this.num = num;
        }

        public Collider collider;
        public int num;
    };

    private Collider AroundRaycast(Vector3 pos)
    {
        List<CheckHitCollider> hitColliderList = new List<CheckHitCollider>();
        RaycastHit checkHit;

        rayArray[0] = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(pos));
        for (int i = 1; i <= 4; i++)
            rayArray[i] = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(CalcCircularPos(pos, checkRadius1, checkRadius1, i)));
        for (int i = 5; i <= 8; i++)
            rayArray[i] = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(CalcCircularPos(pos, checkRadius1, checkRadius2, i)));

        for (int i = 9; i <= 12; i++)
            rayArray[i] = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(CalcCircularPos(pos, checkRadius1 * 0.5f, checkRadius1 * 0.5f, i)));
        for (int i = 13; i <= 16; i++)
            rayArray[i] = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(CalcCircularPos(pos, checkRadius1 * 0.5f, checkRadius2 * 0.5f, i)));

        bool isFirstColliderHit = true;
        foreach (Ray ray in rayArray)
        {
            if(Physics.Raycast(ray, out checkHit, Camera.main.transform.position.y * 1.3f, layerMask))
            {
                isFirstColliderHit = true;
                foreach (CheckHitCollider obj in hitColliderList)
                {
                    if (System.Object.ReferenceEquals(obj.collider, checkHit.collider))
                    {
                        obj.num++;
                        isFirstColliderHit = false;
                        break;
                    }
                }
                if(isFirstColliderHit)
                {
                    CheckHitCollider newObj = new CheckHitCollider(checkHit.collider, 1);
                    hitColliderList.Add(newObj);
                }
            }
        }

        if (hitColliderList.Count == 0)
            return null;

        List<CheckHitCollider> maxHitNumList = new List<CheckHitCollider>();

        bool isMax = true;
        foreach (CheckHitCollider obj in hitColliderList)
        {
            isMax = true;
            foreach(CheckHitCollider maxNumObj in maxHitNumList)
            {
                if (obj.num > maxNumObj.num)
                {
                    maxHitNumList.Clear();
                    break;
                }
                else if (obj.num < maxNumObj.num)
                    isMax = false;
            }
            if (isMax)
                maxHitNumList.Add(obj);
        }

        Collider maxHitCollider = maxHitNumList[0].collider;
        for (int i = 1; i < maxHitNumList.Count; i++)
        {
            if (Vector3.SqrMagnitude(pos - maxHitNumList[i].collider.transform.position) > Vector3.SqrMagnitude(pos - maxHitCollider.transform.position))
                maxHitCollider = maxHitNumList[i].collider;
        }

        return maxHitCollider;
    }

    private Vector3 CalcCircularPos(Vector3 pos, in float radius1, in float radius2, in int direction)
    {
        Vector3 result = new Vector3();
        result = pos;
        switch(direction)
        {
            case 1:
                result.x += radius1;
                break;
            case 2:
                result.x += radius1 * 0.707f;    // root(2)/2 for rcos(pi/4)
                result.y += radius2 * 0.707f;    // root(2)/2 for rsin(pi/4)
                break;
            case 3:
                result.y += radius2;
                break;
            case 4:
                result.x -= radius1 * 0.707f;
                result.y += radius2 * 0.707f;
                break;
            case 5:
                result.x -= radius1;
                break;
            case 6:
                result.x -= radius1 * 0.707f;
                result.y -= radius2 * 0.707f;
                break;
            case 7:
                result.y -= radius2;
                break;
            case 8:
                result.x += radius1 * 0.707f;
                result.y -= radius2 * 0.707f;
                break;
            default:
                break;
        }

        return result;
    }
    public GameObject LocateTower(Vector3 mousePos)
    {
        checker.FixPosition();

        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 correctionPos = realPos;
        correctionPos.z += correction;

        hitCollider = AroundRaycast(correctionPos);
        if (hitCollider != null)
        {
            //locate proper position
            LocateProperPos(hitCollider.transform);

            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = false;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);

            isProperLocate = true;

            if (System.Object.ReferenceEquals(hitCollider, preHitCollider))
                isSkipFrame_locate = true;
            else
                isSkipFrame_locate = false;

            preHitCollider = hitCollider;

            return hitCollider.gameObject.GetComponent<SideColliderBehaviour>().parentObject;
        }
        else
        {
            // locate tower at mousePos(realPos)
            transform.position = new Vector3(correctionPos.x, 0, correctionPos.z);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);

            //update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = true;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);

            isProperLocate = false;

            return null;
        }
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

        checker.FixPosition();

        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (TagManager.Instance.isNotDeployableTag(other.gameObject))
        {
            if (isProperLocate)
                isOverlapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagManager.Instance.isNotDeployableTag(other.gameObject))
        {
            isOverlapped = false;
        }
    }

}