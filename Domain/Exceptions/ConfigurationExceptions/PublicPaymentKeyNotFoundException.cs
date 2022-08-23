namespace Ipe.Domain.Errors;
public class PublicPaymentKeyNotFoundException : BaseException
{
    public PublicPaymentKeyNotFoundException() : base("017", "Public payment key not found") { }
}
