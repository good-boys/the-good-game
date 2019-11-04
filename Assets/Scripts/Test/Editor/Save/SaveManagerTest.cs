using System.IO;
using NUnit.Framework;
using Moq;

public class SaveManagerTest
{
    string filePath = "unit_test.dat";
    Mock<IOManager> mockIOManager;
    Mock<Stream> mockFile;
    GameSave save;

    SaveManager saveManager;

    [SetUp]
    public void Setup()
    {
        mockIOManager = new Mock<IOManager>();
        mockFile = new Mock<Stream>();
        save = new GameSave(1, new Player("test player", 34, 1), 0, 0, new Tutorial[] { });

        saveManager = new SaveManager(filePath,
                                      mockIOManager.Object);

        mockIOManager.Setup(
            files => files.Serialize(It.IsAny<Stream>(), It.IsAny<object>())
        );
        mockIOManager.Setup(
            files => files.Deserialize(It.IsAny<Stream>())
        ).Returns(save);
        mockIOManager.Setup(
            files => files.OpenWrite(It.IsAny<string>())
        ).Returns(mockFile.Object);
        mockIOManager.Setup(
            files => files.OpenRead(It.IsAny<string>())
        ).Returns(mockFile.Object);
        mockIOManager.Setup(files => files.Delete(It.IsAny<string>()));
    }

    [Test]
    public void TestSave()
    {
        saveManager.Save(save);

        mockIOManager.Verify(
            formatter => formatter.Serialize(mockFile.Object, save)
        );
    }

    [Test]
    public void TestLoad_HasSave()
    {
        givenHasSaveFile();

        GameSave loadedSave = saveManager.Load();

        Assert.AreSame(save, loadedSave);
    }

    [Test]
    public void TestLoad_NoSave()
    {
        Assert.Throws<FileNotFoundException>(delegate ()
        {
            saveManager.Load();
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestHasSave(bool hasSave)
    {
        mockIOManager.Setup(files => files.Exists(filePath)).Returns(hasSave);

        Assert.AreEqual(hasSave, saveManager.HasSave());
    }

    [Test]
    public void TestErase_HasSave()
    {
        givenHasSaveFile();

        saveManager.Erase();

        mockIOManager.Verify(files => files.Delete(filePath));
    }

    [Test]
    public void TestErase_NoSave()
    {
        saveManager.Erase();

        mockIOManager.Verify(files => files.Delete(It.IsAny<string>()), Times.Never());
    }

    void givenHasSaveFile()
    {
        mockIOManager.Setup(files => files.Exists(filePath)).Returns(true);
    }
}
