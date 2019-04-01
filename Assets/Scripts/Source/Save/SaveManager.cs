using System.IO;

public class SaveManager
{
    string filePath;
    IOManager ioManager;

    public SaveManager(string filePath) : this(filePath, new IOManager()) {}

    public SaveManager(string filePath, IOManager ioManager)
    {
        this.filePath = filePath;
        this.ioManager = ioManager;
    }

    public void Save(GameSave save)
    {
        Stream file = ioManager.OpenWrite(filePath);
        ioManager.Serialize(file, save);
        file.Close();
    }

    public virtual GameSave Load()
    {
        if(!HasSave())
        {
            throw new FileNotFoundException();
        }
        Stream file = ioManager.OpenRead(filePath);
        GameSave save = ioManager.Deserialize(file) as GameSave;
        file.Close();
        return save;
    }

    public virtual bool HasSave()
    {
        return ioManager.Exists(filePath);
    }

    public void Erase()
    {
        if(!HasSave())
        {
            return;
        }
        ioManager.Delete(filePath);
    }
}
