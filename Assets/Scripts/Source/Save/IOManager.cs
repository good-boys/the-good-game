using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class IOManager 
{
    BinaryFormatter binaryFormatter = new BinaryFormatter();

    public virtual void Serialize(Stream file, object data)
    {
        binaryFormatter.Serialize(file, data);
    }

    public virtual object Deserialize(Stream file)
    {
        return binaryFormatter.Deserialize(file);
    }

    public virtual Stream OpenWrite(string filePath)
    {
        return File.OpenWrite(filePath);
    }

    public virtual Stream OpenRead(string filePath)
    {
        return File.OpenRead(filePath);
    }

    public virtual bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public virtual void Delete(string filePath)
    {
        File.Delete(filePath);
    }
}
