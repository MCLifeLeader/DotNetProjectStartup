using Microsoft.EntityFrameworkCore;
using Startup.Common.Repositories;

namespace Startup.Tests.Common;

[TestFixture(Category = "Unit")]
public class DbContextBaseTests
{
    private class TestDbContext : DbContextBase<TestDbContext>
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
    }

    private DbContextOptions<TestDbContext> _options;
    private TestDbContext _context;

    [SetUp]
    public void SetUp()
    {
        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new TestDbContext(_options);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task SaveChangesAsync_ShouldReturnZero_WhenNoChanges()
    {
        var result = await _context.SaveChangesAsync();
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void SaveChanges_ShouldReturnZero_WhenNoChanges()
    {
        var result = _context.SaveChanges();
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Reload_ShouldNotThrowException_WhenEntityIsNotTracked()
    {
        var entity = new
        {
            Id = 1
        };
        Assert.That(() => _context.Reload(entity), Throws.Nothing);
    }

    [Test]
    public void ClearState_ShouldNotThrowException_WhenEntityIsNotTracked()
    {
        var entity = new
        {
            Id = 1
        };
        Assert.That(() => _context.ClearState(entity), Throws.Nothing);
    }

    [Test]
    public void SetDbContextConfigurationAutoDetectChanges_ShouldSetAutoDetectChangesEnabled()
    {
        _context.SetDbContextConfigurationAutoDetectChanges(true);
        Assert.That(_context.ChangeTracker.AutoDetectChangesEnabled, Is.True);

        _context.SetDbContextConfigurationAutoDetectChanges(false);
        Assert.That(_context.ChangeTracker.AutoDetectChangesEnabled, Is.False);
    }
}