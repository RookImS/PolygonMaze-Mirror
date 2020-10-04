using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankBehaviour : MonoBehaviour
{
    public GameObject realBlank; // prefab

    public float innerRadius;
    private bool isDeployEnable;
    public bool isOverlapped;

    public GameObject LocateBlank(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f))
        {
            Collider objectHit = hit.collider;

            Vector3 rotateAngle = new Vector3(0f, (objectHit.transform.rotation.eulerAngles.y + 180) % 360, 0f);
            transform.eulerAngles = rotateAngle;

            Vector3 direction = transform.rotation * Vector3.back;
            Vector3 targetPoint = direction * ((innerRadius) - 0.1f);

            transform.position = objectHit.transform.position + targetPoint;

            isDeployEnable = true;
            return objectHit.gameObject.transform.parent.gameObject;
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            isDeployEnable = false;

            return null;
        }
    }
    public bool CheckOverlap()
    {
        if (isOverlapped)
            isDeployEnable = false;

        return isOverlapped;
    }
    public bool CheckPath()
    {
        // path tracker를 확인해 길이 막히면 false + isDeployEnable = false
        // 문제 없으면 true
        return true;
    }
    public void DeployTower(GameObject neighborObject)
    {

        if (isDeployEnable)
        {
            //GameObject newTower = Instantiate(realTower, transform.position, transform.rotation);-
            //GameObject neighborObject = hitObject.transform.parent.gameObject;

            Debug.Log("이웃" + neighborObject);
            //Debug.Log("새타워" + newTower);

            //hitObject.GetComponent<Collider>().enabled = false;
            //newTower.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = false;

            //neighborObject.GetComponent<TowerBehaviour>().setNeighbor(newTower);-
            //newTower.GetComponent<TowerBehaviour>().setNeighbor(neighborObject);-

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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral"))
            isOverlapped = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral"))
            isOverlapped = false;
    }

}
