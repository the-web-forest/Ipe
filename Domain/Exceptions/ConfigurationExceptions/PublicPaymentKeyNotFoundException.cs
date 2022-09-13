using System.Runtime.Serialization;

namespace Ipe.Domain.Errors;

[Serializable]
public class PortalConfigurationNotFoundException : BaseException
{
    public PortalConfigurationNotFoundException() : base("017", "Portal configuration not found") { }
    protected PortalConfigurationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
