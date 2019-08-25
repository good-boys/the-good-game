using System;
using UnityEngine;

public enum TutorialStepReceiverDelegateAction
{
    Show,
    Hide,
}

public class TutorialStepReceiverDelegate : MonoBehaviour 
{
    [SerializeField]
    bool startEnabled;

    [SerializeField]
    TutorialStepReceiverDelegateAction setupAction;

    [SerializeField]
    TutorialStepReceiverDelegateAction completeAction;

    private void Awake()
    {
        setupReceiverAction();
        gameObject.SetActive(startEnabled);
    }

    private void setupReceiverAction()
    {
        TutorialStepReceiver receiver = GetComponent<TutorialStepReceiver>();
        receiver.SetUp(getAction(setupAction), getAction(completeAction));
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
