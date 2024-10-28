namespace MovieChallenge.BLL.Exceptions
{
    public class UiFriendlyException : Exception
    {
        public UiFriendlyException()
        {
        }

        public UiFriendlyException(string message)
            : base(message)
        {
        }

        public UiFriendlyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
