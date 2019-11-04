using System;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialStepReceiverDelegateAction
{
    Show,
    Hide,
}

public enum TutorialStepTriggerAction
{
    ButtonPress,
    Event
}

public class TutorialStepReceiverDelegate : MonoBehaviour 
{
    [SerializeField]
    bool startEnabled;

    [SerializeField]
    TutorialStepReceiverDelegateAction setupAction;

    [SerializeField]
    TutorialStepTriggerAction triggerAction;

    [SerializeField]
    TutorialStepReceiverDelegateAction completeAction;

    [SerializeField]
    Button triggerButton;

    [SerializeField]
    string triggerEventName;

    [SerializeField]
    string showOnEvent;

    [SerializeField]
    string hideOnEvent;

    private void Awake()
    {
        TutorialStepReceiver receiver = GetComponent<TutorialStepReceiver>();
        setupReceiverActions(receiver);
        gameObject.SetActive(startEnabled);
    }

    private void setupReceiverActions(TutorialStepReceiver receiver)
    {
        receiver.SetUp(getAction(setupAction), getAction(completeAction));
        if(triggerAction == TutorialStepTriggerAction.ButtonPress && triggerButton)
        {
            triggerButton.onClick.AddListener(receiver.CompleteStep);
        }
        if(triggerAction == TutorialStepTriggerAction.Event && !String.IsNullOrEmpty(triggerEventName))
        {
            receiver.SubscribeToEvents(delegate(string eventName) 
            {
                if(triggerEventName == eventName)
                {
                    receiver.CompleteStep();
                }
            });
        }
        if(!String.IsNullOrEmpty(showOnEvent))
        {
            receiver.SubscribeToEvents(delegate (string eventName)
            {
                if(showOnEvent == eventName)
                {
                    gameObject.SetActive(true);
                }
            });
        }
        if(!String.IsNullOrEmpty(hideOnEvent))
        {
            receiver.SubscribeToEvents(delegate (string eventName)
            {
                if(hideOnEvent == eventName)
                {
                    gameObject.SetActive(false);
                }
            });
        }
    }

    private Action getAction(TutorialStepReceiverDelegateAction action)
    {
        switch(action)
        {
            case TutorialStepReceiverDelegateAction.Show:
                return () =>
                {
                    gameObject.SetActive(true);
                };
            case TutorialStepReceiverDelegateAction.Hide:
                return () =>
                {
                    gameObject.SetActive(false);
                };
            default:
                return null;
        }
    }
}
