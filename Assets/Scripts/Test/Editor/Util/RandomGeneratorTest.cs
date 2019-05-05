using NUnit.Framework;

public class RandomGeneratorTest
{
    [TestCase(0, 1)]
    [TestCase(0, 100)]
    [TestCase(-100, 100)]
    public void TestGenerate(float min, float max)
    {
        RandomGenerator generator = new RandomGenerator();

        float result = generator.Generate(min, max);

        Assert.True(result >= min);
        Assert.True(result <= max);
    }
}
