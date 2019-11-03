using System;
using UnityEngine;

public class TutorialStepReceiver : MonoBehaviour
{
    Action onTriggerStep = delegate { };
    Action onCompleteStep = delegate { };
    public string ID
    {
        get { return id; }
    }

    [SerializeField]
    string id;

    public void SetUp(Action onTriggerAction, Action completeStepCallback)
    {
        onTriggerStep += onTriggerAction;
        onCompleteStep += completeStepCallback;

    }

    public void TriggerStep()
    {
        onTriggerStep();
        onTriggerStep = delegate { };
    }

    public void CompleteStep()
    {
        onCompleteStep();
        onCompleteStep = delegate { };
    }
}
