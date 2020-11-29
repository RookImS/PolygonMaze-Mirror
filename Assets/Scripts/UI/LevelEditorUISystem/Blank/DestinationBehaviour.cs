using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationBehaviour: MonoBehaviour
{
    /* void OnMouseDown()
     * 1. destination object를 클릭하면,
     *    LevelEditorUISystem.instance.GetBlankDeleteUISystem().OnClickBlank(~)를 호출한다.
     */
    private void OnMouseDown()
    {
        LevelEditorUISystem.instance.GetBlankDeleteUISystem().OnClickBlank(this.gameObject,
            BlankDeleteUISystem.BlankSpecific.Destination);
    }
}
