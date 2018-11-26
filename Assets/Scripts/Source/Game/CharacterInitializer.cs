using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    [SerializeField]
    CharacterManager characterManager;

    [SerializeField]
    StatManager statManager;

    [SerializeField]
    EnemyActionManager enemyActionManager;

    void Awake()
    {
        characterManager.Init(statManager, enemyActionManager);
    }
}
