namespace Api.Startup.Example.Model.Ui.CanaryPage;

public enum Status
{
    /// <summary>The resource responded to a check as expected and an exception did not occur.</summary>
    Ok,

    /// <summary>The resource responded to a check with an unexpected value or threw an exception. </summary>
    Warning,

    /// <summary>The resource check took longer than the amount of time allotted or didn't respond at all.</summary>
    Critical
}