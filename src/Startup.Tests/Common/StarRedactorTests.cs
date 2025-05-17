using Startup.Common.Helpers.Filter;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class StarRedactorTests
{
    private StarRedactor _starRedactor;

    [SetUp]
    public void SetUp()
    {
        _starRedactor = new StarRedactor();
    }

    [Test]
    public void Redact_ShouldFillDestinationWithAsterisks()
    {
        // Arrange
        var source = "SensitiveData".AsSpan();
        var destination = new char[source.Length];

        // Act
        var result = _starRedactor.Redact(source, destination);

        // Assert
        Assert.That(result, Is.EqualTo(destination.Length));
        Assert.That(destination, Is.All.EqualTo('*'));
    }

    [Test]
    public void GetRedactedLength_ShouldReturnLengthOfInput()
    {
        // Arrange
        var input = "SensitiveData".AsSpan();

        // Act
        var result = _starRedactor.GetRedactedLength(input);

        // Assert
        Assert.That(result, Is.EqualTo(input.Length));
    }
}