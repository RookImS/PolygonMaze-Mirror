using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public Transform prefab;
    public void MakeObstacle()
    {
        Object.Instantiate(prefab, new Vector3(5.6f, 0.75f, -6.0f), Quaternion.identity);
    }

    public void ChangeSpeed()
    {
        GameObject test = GameObject.FindWithTag("Enemy");
        test.GetComponent<EnemyBehaviour>().ChangeAgentSpeed(0.2f);
    }
}
