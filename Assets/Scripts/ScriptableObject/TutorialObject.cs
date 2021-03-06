using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tutorial00", menuName = "ScriptableObjects/TutorialScriptableObject", order = 2)]
public class TutorialObject : ScriptableObject
{
    [Serializable]
    public class Tutorial
    {
        public Tutorial(Tutorial t)
        {
            dialogue = new Dialogue(t.dialogue);

            animationList = new List<AniObject>();
            foreach (AniObject temp in t.animationList)
                animationList.Add(new AniObject(temp));

            fieldAnimationList = new List<AniObject>();
            foreach (AniObject temp in t.fieldAnimationList)
                fieldAnimationList.Add(new AniObject(temp));

            maskAnimationList = new List<AniObject>();
            foreach (AniObject temp in t.maskAnimationList)
                maskAnimationList.Add(new AniObject(temp));
        }

        public Dialogue dialogue;
        public List<AniObject> animationList;
        public List<AniObject> fieldAnimationList;
        public List<AniObject> maskAnimationList;
        //public AniObject maskAnimation;
    }

    public int chapter;
    public int level;
    public List<Tutorial> tutorialList;
    public TutorialChecker tutorialChecker;
}