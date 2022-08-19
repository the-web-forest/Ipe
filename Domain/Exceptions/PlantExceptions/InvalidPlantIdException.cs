namespace Ipe.Domain.Errors;
public class InvalidPlantIdException : BaseException
{
    public InvalidPlantIdException() : base("014", "Invalid Plant Id") { }
}
