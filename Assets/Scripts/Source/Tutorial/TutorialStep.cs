using System;

using UnityEngine;

[Serializable]
public class TutorialStep
{
    public bool Complete { get; private set; }
    public string ReceiverID
    {
        get { return receiverId; }
    }

    [SerializeField]
    string receiverId;

    public void Finish()
    {
        Complete = true;
    }

    public void Reset()
    {
        Complete = false;
    }
}
