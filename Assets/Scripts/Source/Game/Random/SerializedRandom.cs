using System;

[Serializable]
public class SerializedRandom
{
    int place;
    int seed;
    Random rand;

    public SerializedRandom(int seed, int place)
    {
        this.place = 0;
        moveToPlace(place);
        this.seed = seed;
        rand = new Random(seed);
    }

    void moveToPlace(int newPlace)
    {
        for(int i = place; i < place; i++)
        {
            rand.Next();
        }
        place = newPlace;
    }

    public int Next()
    {
        place++;
        return rand.Next();
    }
}
