using System;

using UnityEngine;

[Serializable]
public class Tutorial
{
    public const string ATTACK_TUTORIAL = "Attack";
    public const string DEFEND_TUTORIAL = "Defend";
    public const string COMBO_TUTORIAL = "Combo";

    public string Name { get; private set; }
    public TutorialStep Current { get { return steps.Length == 0 ? null : steps[currentStepIdx]; } }
    public bool Complete { get; private set; }
    public string[] Prerequisites { get { return prerequisites; } }

    [SerializeField]
    private TutorialStep[] steps;
    [SerializeField]
    private string[] prerequisites;

    private TutorialStep start
    {
        get { return steps[0]; }
    }

    private int currentStepIdx = 0;

    public void SetName(string name)
    {
        Name = name;
    }

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
