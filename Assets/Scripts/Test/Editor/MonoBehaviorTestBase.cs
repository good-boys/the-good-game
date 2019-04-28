using UnityEngine;

public class MonoBehaviorTestBase<T> where T : MonoBehaviour
{
    protected GameObject gameObject;
    protected T testInstance;

    public virtual void Setup()
    {
        gameObject = new GameObject();
        testInstance = gameObject.AddComponent<T>();
    }
}
