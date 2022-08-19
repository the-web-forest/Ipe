using Ipe.Controllers.Plant.DTOs;
using Ipe.Domain.Errors;
using Ipe.UseCases;
using Ipe.UseCases.PlantCustomizeUseCase;
using Ipe.UseCases.PlantUseCase.CreatePlant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ipe.Controllers.Plant;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PlantController : Controller
{
    private readonly ILogger<PlantController> _logger;

    public PlantController(ILogger<PlantController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<ObjectResult> Plant(
        [FromBody] PlantInput Input,
        [FromServices] IUseCase<PlantUseCaseInput, PlantUseCaseOutput> _createPlantUseCase
    )
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _logger.LogInformation("Create plant order for User => {UserId}", userId);

        try
        {
            var Data = await _createPlantUseCase.Run(new PlantUseCaseInput
            {
                UserId = userId,
                CardToken = Input.CardToken,
                Trees = Input.Trees
                    .Select(tree => new TreeUseCaseInput
                    {
                        Id = tree.Id,
                        Quantity = tree.Quantity
                    })
                    .ToList()
            });

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

    [HttpPost("Customize")]
    public async Task<ObjectResult> PlantCustomize(
        [FromBody] PlantCustomizeInput Input,
        [FromServices] IUseCase<PlantCustomizeUseCaseInput, PlantCustomizeUseCaseOutput> _customizePlantUseCase
    )
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _logger.LogInformation("Customize plant order for User => {UserId}", userId);

        try
        {
            if (Input.IsInvalid())
                throw new InvalidPlantCustomizeException();

            var Data = await _customizePlantUseCase
                .Run(new PlantCustomizeUseCaseInput
                {
                    UserId = userId,
                    PlantId = Input.PlantId,
                    TreeName = Input.TreeName,
                    TreeMessage = Input.TreeMessage,
                    TreeHastags = Input.TreeHastags ?? new List<string>()
                });

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