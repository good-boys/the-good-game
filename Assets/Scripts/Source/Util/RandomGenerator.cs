using UnityEngine;

public class RandomGenerator 
{
    public virtual float Generate(float min, float max)
    {
        return Random.Range(0f, 1f);
    }
}
