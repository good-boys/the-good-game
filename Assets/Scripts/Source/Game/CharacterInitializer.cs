using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    StatManager statManager;

    [SerializeField]
    EnemyActionManager enemyActionManager;

    public CharacterInitializer(CharacterManager characterManager, 
                                StatManager statManager, 
                                EnemyActionManager enemyActionManager)
    {
        this.characterManager = characterManager;
        this.statManager = statManager;
        this.enemyActionManager = enemyActionManager;
    }

    public void Awake()
    {
        characterManager.Init(statManager, enemyActionManager);
    }
}
