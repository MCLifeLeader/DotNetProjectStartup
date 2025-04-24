namespace Startup.Common.Helpers.Exceptions
{
    [Serializable]
    public class ProblemException : Exception
    {
        public string Error { get; }

        // ReSharper disable once ConvertToPrimaryConstructor
        public ProblemException(string error, string message) : base(message)
        {
            Error = error;
        }
    }
}