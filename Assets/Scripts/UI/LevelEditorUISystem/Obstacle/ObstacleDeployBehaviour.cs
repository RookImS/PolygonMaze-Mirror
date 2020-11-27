using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class ObstacleDeployBehaviour : MonoBehaviour
{
    public LayerMask layerMask;
    public float innerRadius;
    public Renderer rend;

    private GameObject obtaclesGameObject;
    private CheckerBehaviour checker;
    private NavMeshSurface checkerNav;
    private NavMeshSurface enemyNav;

    private bool isDeployEnable;
    private bool isProperLocate;
    private bool isPathEnable;
    private bool isOverlapped;
    private bool isSkipFrame;

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        isPathEnable = CheckPath();
        isDeployEnable = CheckDeployEnable();
        isSkipFrame = CheckSkipFrame();
    }

    private void Init()
    {
        checkerNav = GameObject.Find("CheckerNavMeshSurface").GetComponent<NavMeshSurface>();
        enemyNav = GameObject.Find("EnemyNavMeshSurface").GetComponent<NavMeshSurface>();
        checker = GameObject.FindGameObjectWithTag("Checker").GetComponent<CheckerBehaviour>();
        obtaclesGameObject = GameObject.Find("Obstacles");

        isDeployEnable = false;
        isProperLocate = false;
        isSkipFrame = true;
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

    private bool CheckSkipFrame()
    {
        if (isProperLocate)
            return false;
        else
            return true;
    }

    private bool CheckDeployEnable()
    {
        if (isPathEnable && isProperLocate && !isOverlapped)
        {
            rend.material.color = new Color(1f, 1f, 1f, 85 / 255f);

            return true;
        }
        else
        {
            if (isProperLocate)
            {
                if (!isSkipFrame)
                {
                    rend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 85 / 255f);
                }
            }
            else
            {
                rend.material.color = new Color(1f, 1f, 1f, 85 / 255f);
            }

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

    public void SetIsDeployEnable(bool flag)
    {
        this.isDeployEnable = flag;
    }

    public GameObject LocateObstacle(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        // locate obstacle at mousePos(realPos)
        transform.position = new Vector3(realPos.x, 0f, realPos.z);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f, layerMask))
        {
            Collider objectHit = hit.collider;

            //locate proper position
            LocateProperPos(objectHit.transform);

            // update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = false;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);
            checker.FixPosition();

            isProperLocate = true;

            return objectHit.gameObject.GetComponent<LevelEditorSideColliderBehaviour>().parentObject;
        }
        else
        {
            //update checker navMesh
            GetComponent<NavMeshModifier>().ignoreFromBuild = true;
            checkerNav.UpdateNavMesh(checkerNav.navMeshData);

            isProperLocate = true;

            return null;
        }
    }

    public void DeployObstacle(GameObject realObstacle)
    {
        if (isDeployEnable)
        {
            GameObject newObstacle = Instantiate(realObstacle, transform.position, transform.rotation, obtaclesGameObject.transform);

            LevelEditor.instance.GetObstacleList().Add(newObstacle);
            if (LevelEditor.instance.GetObstacleList().Count > 0)
            {
                LevelEditorUISystem.instance.ChangeButtonColor(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                    , LevelEditorUISystem.ButtonColor.ReadyColor);

                LevelEditorUISystem.instance.ChangeReadyFlag(LevelEditorUISystem.SettingUISystemSpecific.ObstacleSetting
                , true);
            }

            // update enemy navMesh
            enemyNav.UpdateNavMesh(enemyNav.navMeshData);
            checker.ApplyPath();
        }

        // update checker navMesh
        checkerNav.UpdateNavMesh(checkerNav.navMeshData);

        checker.FixPosition();

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
