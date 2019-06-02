using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour 
{
    // TODO: break tutorials into multiple ones (attack, defend, combo, etc)
    public Tutorial CombatTutorial 
    {
        get { return gameData.GameSave.CombatTutorial; }
    }

    [SerializeField]
    DataInitializer gameData;

    [SerializeField]
    TutorialStepReceiver[] tutorialStepReceivers;

    Dictionary<string, TutorialStepReceiver> receiverMap;

    private void Start()
    {
        if(!gameData)
        {
            Debug.LogError("DataInitialier instance not set for TutorialManager");
            return;
        }
        populateReceiverMap();
    }

    private void populateReceiverMap()
    {
        receiverMap = new Dictionary<string, TutorialStepReceiver>();
        foreach(TutorialStepReceiver receiver in tutorialStepReceivers)
        {
            receiverMap[receiver.ID] = receiver;
        }
    }

    public void TriggerStep(Tutorial tutorial)
    {
        TutorialStepReceiver receiver;
        if(!receiverMap.TryGetValue(tutorial.Current.ReceiverID, out receiver))
        {
            Debug.LogErrorFormat("Unable to run step {0}. No receiver found", tutorial.Current.ReceiverID);
            return;
        }

        receiver.SetUp(delegate
        {
            Debug.LogFormat("[{0}]: STEP COMPLETED", tutorial.Current.ReceiverID);
            CompleteStep(tutorial);
            gameData.SaveCurrent();
        });
    }

    public void CompleteStep(Tutorial tutorial)
    {
        tutorial.Step();
        if(!tutorial.Complete)
        {
            TriggerStep(tutorial);
        }
    }
}
