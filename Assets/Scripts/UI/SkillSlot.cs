using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillSlot : MonoBehaviour
{
    public GameObject Slot;

    private void Start()
    {
        this.transform.GetChild(0).GetComponent<Image>().color = Slot.GetComponent<Skill>().color;
        this.GetComponent<Button>().onClick.AddListener(InitSlot);
    }
    void InitSlot()
    {
        Deck.instance.viewPanel(Slot);
    }
    
}
