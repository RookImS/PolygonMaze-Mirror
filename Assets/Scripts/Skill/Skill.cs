using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int cost;

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public virtual void UseSkill(Vector3 mousePos)
    {

    }
}
