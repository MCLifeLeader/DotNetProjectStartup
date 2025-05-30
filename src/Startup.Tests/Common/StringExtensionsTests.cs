using Startup.Common.Helpers.Extensions;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class StringExtensionsTests
{
    [Test]
    public void ScrubUrlRoute_WithDoubleSlashes_ReplacesWithSingleSlash()
    {
        // Arrange
        var url = "http://example.com//path//to//resource";

        // Act
        var result = url.ScrubUrlRoute();

        // Assert
        Assert.That(result, Is.EqualTo("http://example.com/path/to/resource"));
    }

    [Test]
    public void ScrubUrlRoute_WithNoDoubleSlashes_ReturnsSameUrl()
    {
        // Arrange
        var url = "http://example.com/path/to/resource";

        // Act
        var result = url.ScrubUrlRoute();

        // Assert
        Assert.That(result, Is.EqualTo(url));
    }

    [Test]
    public void ScrubUrlRoute_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var url = string.Empty;

        // Act
        var result = url.ScrubUrlRoute();

        // Assert
        Assert.That(result, Is.EqualTo(url));
    }

    [Test]
    public void Mask_WithStringLongerThanFourCharacters_MasksCorrectly()
    {
        // Arrange
        var input = "1234567890";

        // Act
        var result = input.Mask();

        // Assert
        Assert.That(result, Is.EqualTo("1234********"));
    }

    [Test]
    public void Mask_WithStringOfFourCharacters_ReturnsMaskedString()
    {
        // Arrange
        var input = "1234";

        // Act
        var result = input.Mask();

        // Assert
        Assert.That(result, Is.EqualTo("****"));
    }

    [Test]
    public void Mask_WithEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = input.Mask();

        // Assert
        Assert.That(result, Is.EqualTo(input));
    }

    [Test]
    public void Mask_WithNullString_ReturnsNull()
    {
        // Arrange
        string? input = null;

        // Act
        var result = input.Mask();

        // Assert
        Assert.That(result, Is.Null);
    }
}