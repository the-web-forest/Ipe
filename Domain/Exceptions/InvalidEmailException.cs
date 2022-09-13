using System.Runtime.Serialization;

namespace Ipe.Domain.Errors;

[Serializable]
public class InvalidEmailException : BaseException
{
    public InvalidEmailException() : base("008", "Invalid Email") { }
    protected InvalidEmailException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
