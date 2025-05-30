using System.ComponentModel.DataAnnotations;

namespace Startup.Api.Models.Ui.InfoPage;

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