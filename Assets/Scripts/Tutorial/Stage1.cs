using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : TutorialChecker
{
    public override bool StartCheck(int phase)
    {
        switch(phase)
        {
            case 6:
                return phase6Checker();
            case 9:
                return phase9Checker();
            case 11:
                return phase11Checker();
            default:
                return true;
        }
    }

    private bool phase6Checker()
    {
        return false;
    }

    private bool phase9Checker()
    {
        return false;
    }
    private bool phase11Checker()
    {
        return false;
    }
}
