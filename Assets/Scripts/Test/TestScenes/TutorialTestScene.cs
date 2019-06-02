using UnityEngine;
using System.Collections;

public class TutorialTestScene : MonoBehaviour 
{
    [SerializeField]
    TutorialManager tutorialManager;

    [SerializeField]
    DataInitializer dataInitializer;

    private void Start()
    {
        StartCoroutine(waitAndStartTutorial());
    }

    IEnumerator waitAndStartTutorial()
    {
        yield return new WaitForEndOfFrame();
        Tutorial combatTutorial = tutorialManager.CombatTutorial;
        Debug.LogFormat("Current step id: {0}", combatTutorial.Current.ReceiverID);
        Debug.LogFormat("Current step is complete: {0}", combatTutorial.Current.Complete);
        Debug.LogFormat("Tutorial is complete: {0}", combatTutorial.Complete);
        tutorialManager.TriggerStep(combatTutorial);
        dataInitializer.SaveCurrent();
    }


    public void ResetTutorialProgress()
    {
        dataInitializer.SaveManager.Erase();
        dataInitializer.SetUp();
    }
}
