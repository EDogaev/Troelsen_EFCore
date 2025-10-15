namespace AutoLot.Dal.Tests;

// Тестовые методы без параметров называются фактами (и задействуют атрибут [Fact]).
// Тестовые методы , которые принимают параметры , называются теориями (они используют атрибут [Theory])
// и могут выполнять множество итераций с разными значениями, передаваемыми в качестве параметров.
public class SampleTests
{
    [Fact]
    public void SimpleFactTest()
    {
        Assert.Equal(5, 2 + 3);
    }

    [Theory]
    [InlineData(3, 2, 5)]
    [InlineData(1, -1, 0)]
    public void SimpleTheoryTest(int addend1, int addend2, int expectedResult)
    {
        Assert.Equal(expectedResult, addend1 + addend2);
    }
}