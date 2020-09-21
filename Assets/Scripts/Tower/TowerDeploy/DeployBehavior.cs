using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeployBehavior : MonoBehaviour
{
    public GameObject realTower;
    public LayerMask layerMask;

    public float innerRadius;
    private bool isDeployEnable;

    public void LocateTower(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        transform.position = new Vector3(pos.x, 0.75f, pos.z);

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f, layerMask))
        {
            Collider objectHit = hit.collider;

            Vector3 rotateAngle = new Vector3(0f, (objectHit.transform.rotation.eulerAngles.y + 180) % 360, 0f);
            transform.eulerAngles = rotateAngle;

            Vector3 direction = transform.rotation * Vector3.back;
            Vector3 targetPoint = direction * (innerRadius);

            transform.position = objectHit.transform.position + targetPoint;

            isDeployEnable = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            isDeployEnable = false;
        }

    }
    public bool CheckOverlap()
    {
        if (transform.GetChild(1).GetComponent<CheckOverlap>().isOverlapped)
            isDeployEnable = false;

        return transform.GetChild(1).GetComponent<CheckOverlap>().isOverlapped;
    }
    public bool CheckPath()
    {
        // path tracker를 확인해 길이 막히면 false + isDeployEnable = false
        // 문제 없으면 true
        return true;
    }
    public void DeployTower(Vector3 pos)
    {
        if (isDeployEnable)
        {
            Instantiate(realTower, pos, transform.rotation);
            /*
            if (PlayerControl.Instance.UseCost(realTower.GetComponent<TowerData>().cost))
                Instantiate(realTower, pos, transform.rotation);
            
            else
            {
                // 부족하다고 알리는 트리거
            }
            */
        }
        Destroy(this.gameObject);
    }

}