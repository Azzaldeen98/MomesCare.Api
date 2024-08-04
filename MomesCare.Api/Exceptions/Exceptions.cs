namespace MomesCare.Api.Exceptions
{
    public class AgeGroupException : NullReferenceException
    {
        public AgeGroupException() : base() { }
        public AgeGroupException(string message) : base(message) { }
    }

    public class CareTypeException : NullReferenceException
    {
        public CareTypeException() : base() { }
        public CareTypeException(string message) : base(message) { }
    }
}
