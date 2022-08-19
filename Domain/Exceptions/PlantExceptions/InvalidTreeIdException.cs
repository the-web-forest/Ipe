namespace Ipe.Domain.Errors;
public class InvalidTreeIdException : BaseException
{
    public InvalidTreeIdException() : base("012", "Invalid Tree Id") { }
}