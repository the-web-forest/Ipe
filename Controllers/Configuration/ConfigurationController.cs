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

    public ConfigurationController(ILogger<PlantController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ObjectResult> PaymentPublicKey(
        [FromServices] IUseCase<ConfigurationPaymentUseCaseInput, ConfigurationPaymentUseCaseOutput> _configurationUseCase
    )
    {
        _logger.LogInformation("Payment Public Key Requested");

        try
        {
            var Data = await _configurationUseCase
                .Run(new ConfigurationPaymentUseCaseInput());

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
