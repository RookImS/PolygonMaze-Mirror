using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : MonoBehaviour
{
    private Renderer rend;
    Color fadingColor;
    float fadingAmount;

    private void Update()
    {
        fadingColor.a -= fadingAmount;
        rend.material.color = fadingColor;

        if (rend.material.color.a <= 0)
            Destroy(this.gameObject);
    }

    public void Init(float duration)
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
        fadingColor = rend.material.color;
        fadingAmount = 1f / duration;
        fadingAmount *= Time.deltaTime;
    }
}
