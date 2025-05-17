using Startup.Common.Helpers.Extensions;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class DictionaryExtensionsTests
{
    [Test]
    public void GetOrAdd_ShouldAddValue_WhenKeyDoesNotExist()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();
        string key = "testKey";
        int value = 42;

        // Act
        var result = dictionary.GetOrAdd(key, value);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(value));
            Assert.That(dictionary.ContainsKey(key), Is.True);
            Assert.That(dictionary[key], Is.EqualTo(value));
        });
    }

    [Test]
    public void GetOrAdd_ShouldReturnExistingValue_WhenKeyExists()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>
        {
            {
                "testKey", 1
            }
        };
        string key = "testKey";
        int value = 42;

        // Act
        var result = dictionary.GetOrAdd(key, value);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(1));
            Assert.That(dictionary[key], Is.EqualTo(1));
        });
    }

    [Test]
    public void TryUpdate_ShouldUpdateValue_WhenKeyExists()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>
        {
            {
                "testKey", 1
            }
        };
        string key = "testKey";
        int newValue = 42;

        // Act
        var result = dictionary.TryUpdate(key, newValue);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(dictionary[key], Is.EqualTo(newValue));
        });
    }

    [Test]
    public void TryUpdate_ShouldReturnFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();
        string key = "testKey";
        int newValue = 42;

        // Act
        var result = dictionary.TryUpdate(key, newValue);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(dictionary.ContainsKey(key), Is.False);
        });
    }
}