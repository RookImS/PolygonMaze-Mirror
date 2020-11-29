using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectionUI : MonoBehaviour
{
    public void Deck(int i)
    {
        if (!GameManager.DeckList[i].Contains(null))
        {
            GameManager.Instance.PlayableDeck = GameManager.DeckList[i];
            for (int a = 0; a < 4; a++)
            {
                Debug.Log(GameManager.Instance.PlayableDeck[a].GetComponent<Skill>().itemName);
            }
            Debug.Log("스킬이 등록되었습니다.");
        }
        else
        {
            Debug.Log("스킬의 수가 부족하다.");
        }
    }
}
