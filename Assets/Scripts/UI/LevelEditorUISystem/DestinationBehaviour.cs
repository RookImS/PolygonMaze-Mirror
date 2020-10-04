using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBehaviour: MonoBehaviour
{
    private void OnMouseDown()
    {
        LevelEditorUISystem.instance.ClickBlank(this.gameObject, 2);
    }
}
