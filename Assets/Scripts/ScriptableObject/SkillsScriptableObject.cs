using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "skill", menuName = "ScriptableObjects/SkillsScriptableObject", order = 3)]
public class SkillsScriptableObject : ScriptableObject
{
    public List<GameObject> skillList;

    //public static SkillsScriptableObject Create()
    //{
    //    SkillsScriptableObject asset = SkillsScriptableObject.CreateInstance<SkillsScriptableObject>();

    //    AssetDatabase.CreateAsset(asset, "Assets")
    //}
}