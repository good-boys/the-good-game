public class Game 
{
    public bool Running
    {
        get 
        {
            return running;
        }
    }

    bool running;

    public Game() 
    {
        running = true;
    }
}
