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
    TutorialStepReceiverDelegateAction action;

    private void Start()
    {
        setupReceiverAction();
    }

    private void setupReceiverAction()
    {
        TutorialStepReceiver receiver = GetComponent<TutorialStepReceiver>();
        receiver.SetUp(getAction());
    }

    private Action getAction()
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
