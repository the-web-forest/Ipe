using Ipe.Domain.Errors;
namespace Ipe.UseCases.ConfigurationPaymentPublicKeyUseCase;

public class GetConfigurationUseCase : IUseCase<GetConfigurationUseCaseInput, GetConfigurationUseCaseOutput>
{
    private IConfiguration _configuration { get; }

    public GetConfigurationUseCase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<GetConfigurationUseCaseOutput> Run(GetConfigurationUseCaseInput Input)
    {
        string? configuration = _configuration["Portal:Configuration"];

        if (string.IsNullOrEmpty(configuration))
            throw new PortalConfigurationNotFoundException();

        return Task.FromResult(new GetConfigurationUseCaseOutput
        {
            Config = configuration
        });
    }
}