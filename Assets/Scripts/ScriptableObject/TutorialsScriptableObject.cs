using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialList", menuName = "ScriptableObjects/TutorialsScriptableObject", order = 4)]
public class TutorialsScriptableObject : ScriptableObject
{
    public List<TutorialObject> ingameTutorialList;
    public List<TutorialObject> outgameTutorialList;
}
