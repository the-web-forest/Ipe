using System.Runtime.Serialization;

namespace Ipe.Domain.Errors;

[Serializable]
public class BaseException : Exception
{
    protected BaseException() { }

    protected BaseException(string Code, string Message)
    {
        Data.Add("Code", "IPE-" + Code);
        Data.Add("Message", Message);
        Data.Add("ShortMessage", Message.Replace(" ", string.Empty));
    }

    protected BaseException(string message, Exception innerException)
        : base(message, innerException)
    { }

    protected BaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
