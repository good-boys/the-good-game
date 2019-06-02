using System;
using UnityEngine;

public class TutorialStepReceiver : MonoBehaviour
{
    Action onCompleteStep = delegate { };
    public string ID
    {
        get { return id; }
    }

    [SerializeField]
    string id;

    public void SetUp(Action completeStepCallback)
    {
        onCompleteStep += completeStepCallback;
        // TODO
        Debug.LogFormat("[{0}]: RUNNING SETUP", id);
    }

    public void CompleteStep()
    {
        onCompleteStep();
        onCompleteStep = delegate { };
    }
}
