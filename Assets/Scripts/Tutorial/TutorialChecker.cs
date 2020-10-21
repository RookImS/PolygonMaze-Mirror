using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour
{
    public virtual bool StartCheck(int phase)
    {
        return false;
    }
}
