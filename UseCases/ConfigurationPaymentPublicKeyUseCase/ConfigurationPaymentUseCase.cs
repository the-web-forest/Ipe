using Ipe.Domain.Errors;

namespace Ipe.UseCases.ConfigurationPaymentPublicKeyUseCase;
public class ConfigurationPaymentUseCase : IUseCase<ConfigurationPaymentUseCaseInput, ConfigurationPaymentUseCaseOutput>
{
    private IConfiguration _configuration { get; }

    public ConfigurationPaymentUseCase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<ConfigurationPaymentUseCaseOutput> Run(ConfigurationPaymentUseCaseInput Input)
    {
        string? paymentPublicKey = _configuration[Input.ConfigurationKey_PaymentPublicKey];

        if (string.IsNullOrEmpty(paymentPublicKey))
            throw new PublicPaymentKeyNotFoundException();

        return Task.FromResult(new ConfigurationPaymentUseCaseOutput
        {
            Settings = new List<ConfigurationSettingsUseCase>
            {
                new ConfigurationSettingsUseCase
                {
                    PaymentPublicKey = paymentPublicKey
                }
            }
        });
    }
}