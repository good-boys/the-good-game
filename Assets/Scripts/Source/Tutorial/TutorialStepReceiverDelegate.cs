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
