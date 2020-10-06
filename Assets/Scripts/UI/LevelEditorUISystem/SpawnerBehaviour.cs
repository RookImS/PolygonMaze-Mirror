using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour: MonoBehaviour
{
    private void OnMouseDown()
    {
        LevelEditorUISystem.instance.GetBlankSettingUISystem().OnClickBlank(this.gameObject,
            BlankSettingUISystem.BlankSpecific.Spawner);
    }
}
