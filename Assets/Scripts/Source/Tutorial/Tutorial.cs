using System;

using UnityEngine;

[Serializable]
public class Tutorial
{
    public TutorialStep Current { get { return steps[currentStepIdx]; } }
    public bool Complete { get; private set; }

    [SerializeField]
    private TutorialStep[] steps;

    private TutorialStep start
    {
        get { return steps[0]; }
    }

    private int currentStepIdx = 0;

    public void Step()
    {
        Current.Finish();
        if(currentStepIdx < steps.Length - 1)
        {
            currentStepIdx++;
        }
        else
        {
            Complete = true;
        }
    }

    public void Reset()
    {
        Complete = false;
        currentStepIdx = 0;
        if(steps != null && steps.Length > 0)
        {
            start.Reset();
        }
    }
}
