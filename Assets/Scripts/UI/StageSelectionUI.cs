using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectionUI : MonoBehaviour
{
    public void Deck(int i)
    {
        GameManager.Instance.PlayableDeck = GameManager.DeckList[i];
        for (int a=0; a < 4; a++)
        {
            Debug.Log(GameManager.Instance.PlayableDeck[a].itemName);
        }
    }
}
