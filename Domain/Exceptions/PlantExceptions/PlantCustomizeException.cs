namespace Ipe.Domain.Errors;
public class PlantCustomizeException : BaseException
{
    public PlantCustomizeException() : base("015", "Customization of unauthorized planting") { }
}
