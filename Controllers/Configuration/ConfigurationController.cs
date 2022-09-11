using Ipe.Controllers.Plant;
using Ipe.Domain.Errors;
using Ipe.UseCases;
using Ipe.UseCases.ConfigurationPaymentPublicKeyUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ipe.Controllers.Configuration;
[ApiController]
[Route("[controller]")]
public class ConfigurationController : Controller
{
    private readonly ILogger<PlantController> _logger;
    private readonly IUseCase<GetConfigurationUseCaseInput, GetConfigurationUseCaseOutput> _configurationUseCase;

    public ConfigurationController(ILogger<PlantController> logger, IUseCase<GetConfigurationUseCaseInput, GetConfigurationUseCaseOutput> configurationUseCase)
    {
        _logger = logger;
        _configurationUseCase = configurationUseCase;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ObjectResult> PaymentPublicKey()
    {
        _logger.LogInformation("Configuration Requested");

        try
        {
            var Data = await _configurationUseCase
                .Run(new GetConfigurationUseCaseInput());

            return new ObjectResult(Data);
        }
        catch (BaseException e)
        {
            return new BadRequestObjectResult(e.Data);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
