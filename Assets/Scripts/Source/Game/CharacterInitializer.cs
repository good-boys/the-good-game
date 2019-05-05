using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    StatManager statManager;

    [SerializeField]
    EnemyActionManager enemyActionManager;

    public void Init(CharacterManager characterManager, 
                                StatManager statManager, 
                                EnemyActionManager enemyActionManager)
    {
        this.characterManager = characterManager;
        this.statManager = statManager;
        this.enemyActionManager = enemyActionManager;
    }

    public void Awake()
    {
        enemyActionManager.Init(new RandomGenerator());
        characterManager.Init(statManager, enemyActionManager);
    }
}
