using System.Runtime.Serialization;

namespace Ipe.Domain.Errors;

[Serializable]
public class PublicPaymentKeyNotFoundException : BaseException
{
    public PublicPaymentKeyNotFoundException() : base("017", "Public payment key not found") { }
    protected PublicPaymentKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
