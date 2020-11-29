using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public int cost;
    public string itemName;
    public int itemValue;
    public int itemPrice;
    public string itemDesc;
    public Sprite itemImage;
    public Color skillColor;

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public virtual void UseSkill(Vector3 mousePos)
    {

    }

    public virtual void SkillDesc(string _itemName, int _itemValue, int _itemPrice, string _itemDesc, Sprite _itemImage)
    {

    }
}
