namespace Ipe.Domain.Errors;
public class PlantCustomizePerformedException : BaseException
{
    public PlantCustomizePerformedException() : base("016", "Customization already done") { }
}
