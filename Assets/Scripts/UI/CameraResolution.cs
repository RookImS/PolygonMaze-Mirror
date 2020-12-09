using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public Camera letterBoxObject;

    private void Start()
    {
        SetAspect(16f, 9f);
    }

    private void SetAspect(float wRatio, float hRatio)
    {
        //메인 카메라의 비율 변경을 위해 받아옵니다.
        Camera mainCam = Camera.main;

        Rect mainRect = mainCam.rect;
        float scaleheight = ((float)Screen.width / (float)Screen.height) / (wRatio / hRatio);
        float scalewidth = 1f / scaleheight;

        //레터박스. 레터박스는 화면을 렌더하지않는 카메라 프리팹입니다.
        //float letterX = 0f;
        //float letterY = 0f;
        //float letterWidth = 1f;
        //float letterHeight = 1f;

        // 모든 rect좌표는 왼쪽 아래가 (0,0)
        if (scaleheight < 1)    // height > width
        {
            // apply maincam
            mainRect.height = scaleheight;
            mainRect.y = (1f - scaleheight) / 2f;

            // apply letterCam
            //letterHeight = (1f - mainRect.height) / 2f;
            //letterRect1 = new Rect(letterX, letterY, letterWidth, letterHeight);
            //letterY = 1f - mainRect.y;
            //letterRect2 = new Rect(letterX, letterY, letterWidth, letterHeight);
        }
        else                    // height <= width
        {
            // apply maincam
            mainRect.width = scalewidth;
            mainRect.x = (1f - scalewidth) / 2f;

            // apply letterCam
            //letterWidth = (1f - mainRect.width) / 2f;
            //letterRect1 = new Rect(letterX, letterY, letterWidth, letterHeight);
            //letterX = 1f - mainRect.x;
            //letterRect2 = new Rect(letterX, letterY, letterWidth, letterHeight);
        }

        Vector3 direction = transform.rotation * Vector3.forward;
        Vector3 backward = transform.rotation.eulerAngles;

        if (direction.x != 0)
        {
            backward.z += 180;
        }
        else if(direction.y != 0)
        {
            backward.x += 180;
        }
        else
        {
            backward.y += 180;
        }
        
        Camera letterBoxCam = Instantiate(letterBoxObject, transform.position, Quaternion.Euler(backward), transform);

        mainCam.rect = mainRect;
        letterBoxCam.depth = mainCam.depth - 1;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}