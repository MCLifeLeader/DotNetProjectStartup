using Startup.Common.Helpers;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class GuidFunctionsTests
{
    [Test]
    public void IsGreaterThan_LeftGuidGreaterThanRightGuid_ReturnsTrue()
    {
        // Arrange
        Guid left = Guid.NewGuid();
        Guid right = Guid.NewGuid();

        // Act
        bool result = left.IsGreaterThan(right);

        // Assert
        Assert.That(result, Is.EqualTo(left.CompareTo(right) > 0));
    }

    [Test]
    public void IsGreaterThanOrEqual_LeftGuidGreaterThanOrEqualToRightGuid_ReturnsTrue()
    {
        // Arrange
        Guid left = Guid.NewGuid();
        Guid right = Guid.NewGuid();

        // Act
        bool result = left.IsGreaterThanOrEqual(right);

        // Assert
        Assert.That(result, Is.EqualTo(left.CompareTo(right) >= 0));
    }

    [Test]
    public void IsLessThan_LeftGuidLessThanRightGuid_ReturnsTrue()
    {
        // Arrange
        Guid left = Guid.NewGuid();
        Guid right = Guid.NewGuid();

        // Act
        bool result = left.IsLessThan(right);

        // Assert
        Assert.That(result, Is.EqualTo(left.CompareTo(right) < 0));
    }

    [Test]
    public void IsLessThanOrEqual_LeftGuidLessThanOrEqualToRightGuid_ReturnsTrue()
    {
        // Arrange
        Guid left = Guid.NewGuid();
        Guid right = Guid.NewGuid();

        // Act
        bool result = left.IsLessThanOrEqual(right);

        // Assert
        Assert.That(result, Is.EqualTo(left.CompareTo(right) <= 0));
    }
}