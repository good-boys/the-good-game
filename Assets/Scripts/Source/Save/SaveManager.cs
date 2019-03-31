using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager
{
    string filePath;
    BinaryFormatter binaryFormatter;

    public SaveManager(string filePath)
    {
        this.filePath = filePath;
        binaryFormatter = new BinaryFormatter();
    }

    public void Save(GameSave save)
    {
        FileStream file = File.OpenWrite(filePath);
        binaryFormatter.Serialize(file, save);
        file.Close();
    }

    public GameSave Load()
    {
        if(!HasSave())
        {
            throw new FileNotFoundException();
        }
        FileStream file = File.OpenRead(filePath);
        GameSave save = binaryFormatter.Deserialize(file) as GameSave;
        file.Close();
        return save;
    }

    public bool HasSave()
    {
        return File.Exists(filePath);
    }
}
