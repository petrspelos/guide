namespace Guide.Core.Exceptions
{
    public class InvalidInputParameterException : DomainException
    {
        public InvalidInputParameterException(string message) : base(message) { }
    }
}
