using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour: MonoBehaviour
{
    private void OnMouseDown()
    {
        LevelEditorUISystem.instance.ClickBlank(this.gameObject, 1);
    }
}
