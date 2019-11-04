using System;
using UnityEngine;

public class TutorialStepReceiver : MonoBehaviour
{
    Action onTriggerStep = delegate { };
    Action onCompleteStep = delegate { };
    Action<string> onEventReceived = delegate (string eventName) {};
    bool stepIsActive = false;

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
        stepIsActive = true;
        onTriggerStep();
        onTriggerStep = delegate { };
    }

    public void CompleteStep()
    {
        stepIsActive = false;
        onCompleteStep();
        onCompleteStep = delegate { };
    }

    public void ReceiveEvent(string eventName)
    {
        if(stepIsActive)
        {
            onEventReceived(eventName);
        }
    }

    public void SubscribeToEvents(Action<string> onEventReceived)
    {
        this.onEventReceived += onEventReceived;
    }
}
