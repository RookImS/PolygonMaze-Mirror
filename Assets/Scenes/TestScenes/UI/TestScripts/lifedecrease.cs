using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lifedecrease : MonoBehaviour
{
    Collider Destination;
    private float Health;
    public Text myText;

    // Update is called once per frame
    public void OnTriggerEnter(Collider Destination)
    {
        Debug.Log("충돌했다.");
        Health = float.Parse(myText.text);
        Debug.Log("남은체력 : "+ Health);
    }
}
