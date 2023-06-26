using System.ComponentModel.DataAnnotations;

namespace Api.Startup.Example.Model.Ui.CanaryPage;

/// <summary>
/// Class TestEntity.
/// </summary>
public class TestEntity
{
    /// <summary>
    /// Gets or sets the test int.
    /// </summary>
    /// <value>The test int.</value>
    [Key]
    public int TestInt { get; set; }
}