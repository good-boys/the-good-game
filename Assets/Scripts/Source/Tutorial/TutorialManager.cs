using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour 
{
    // TODO: break tutorials into multiple ones (attack, defend, combo, etc)
    public Dictionary<string, Tutorial> Tutorials
    {
        get { return tutorialMap; }
    }

    [SerializeField]
    bool debugLogsEnabled = false;

    [SerializeField]
    DataInitializer gameData;

    [SerializeField]
    TutorialStepReceiver[] tutorialStepReceivers;

    Dictionary<string, Tutorial> tutorialMap;
    Dictionary<string, TutorialStepReceiver> receiverMap;
    Action onTutorialComplete = delegate() { };

    public List<Tutorial> AvailableTutorials
    {
        get
        {
            return tutorialMap.Values.Where((tut) => !tut.Complete && prerequisitesSatisified(tut)).ToList();
        }
    }

    private bool prerequisitesSatisified(Tutorial tut)
    {
        foreach(string prereq in tut.Prerequisites)
        {
            if(tutorialMap[prereq] == null)
            {
                Debug.LogWarningFormat("Prerequisite {0} not found for tutorial {1}.", prereq, tut);
                return false;
            }
            if(!tutorialMap[prereq].Complete)
            {
                return false;
            }
        }
        return true;
    }

    private void Start()
    {
        if(!gameData)
        {
            Debug.LogError("DataInitialier instance not set for TutorialManager");
            return;
        }
        populateTutorialMap();
        populateReceiverMap();
    }

    private void populateTutorialMap()
    {
        tutorialMap = new Dictionary<string, Tutorial>();
        foreach(Tutorial tut in gameData.GameSave.Tutorials)
        {
            tutorialMap[tut.Name] = tut;
        }
    }

    private void populateReceiverMap()
    {
        receiverMap = new Dictionary<string, TutorialStepReceiver>();
        foreach(TutorialStepReceiver receiver in tutorialStepReceivers)
        {
            receiverMap[receiver.ID] = receiver;
        }
    }

    bool getReceiver(Tutorial tutorial, out TutorialStepReceiver receiver)
    {
        if(!receiverMap.TryGetValue(tutorial.Current.ReceiverID, out receiver))
        {
            Debug.LogErrorFormat("Unable to find receiver for step {0}.", tutorial.Current.ReceiverID);
            return false;
        }
        return true;
    }

    public void TriggerStep(Tutorial tutorial)
    {
        if(tutorial.Current == null)
        {
            return;
        }

        debugIfEnabled("Trying to trigger tutorial step {0}", tutorial.Current);
        TutorialStepReceiver receiver;
        if(!getReceiver(tutorial, out receiver))
        {
            return;
        }

        receiver.SetUp(
            delegate
            {
                debugIfEnabled("[{0}]: STEP TRIGGERED", tutorial.Current.ReceiverID);
            },
            delegate
            {
                debugIfEnabled("[{0}]: STEP COMPLETED", tutorial.Current.ReceiverID);
                CompleteStep(tutorial);
                gameData.SaveCurrent();
            });
        receiver.TriggerStep();
    }

    public void CompleteStep(Tutorial tutorial)
    {
        tutorial.Step();

        if(tutorial.Complete)
        {
            onTutorialComplete();
        }
        else 
        {
            TriggerStep(tutorial);
        }
    }

    public void OnTutorialComplete(Action onComplete)
    {
        onTutorialComplete += onComplete;
    }

    void debugIfEnabled(string message, params object[] varArgs)
    {
        if(debugLogsEnabled)
        {
            Debug.LogFormat(message, varArgs);
        }
    }
}
