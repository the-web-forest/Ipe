using Ipe.Controllers.Trees.DTOs;
using Ipe.Domain.Errors;
using Ipe.UseCases;
using Ipe.UseCases.TreeUseCase.GetActiveTreeBiomes;
using Ipe.UseCases.TreeUseCase.GetTreesByFilter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ipe.Controllers.Trees;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TreeController : Controller
{
    private readonly ILogger<TreeController> logger;
    private readonly IUseCase<GetTreesByFilterUseCaseInput, GetTreesByFilterUseCaseOutput> getTreesByFilterUseCase;
    private readonly IUseCase<GetActiveTreeBiomesUseCaseInput, GetActiveTreeBiomesUseCaseOutput> getActiveTreeBiomesUseCase;

    public TreeController(
        ILogger<TreeController> logger,
        IUseCase<GetTreesByFilterUseCaseInput, GetTreesByFilterUseCaseOutput> getTreesByFilterUseCase,
        IUseCase<GetActiveTreeBiomesUseCaseInput, GetActiveTreeBiomesUseCaseOutput> getActiveTreeBiomesUseCase)
    {
        this.logger = logger;
        this.getTreesByFilterUseCase = getTreesByFilterUseCase;
        this.getActiveTreeBiomesUseCase = getActiveTreeBiomesUseCase;
    }

    [HttpGet]
    public async Task<ObjectResult> GetTreesByBiome([FromQuery] TreeSearchInput filter)
    {
        logger.LogInformation("Getting trees by filter");

        try
        {
            var trees = await getTreesByFilterUseCase
                .Run(new GetTreesByFilterUseCaseInput
                {
                    Biome = filter.Biome,
                    Name = filter.Name,
                    Description = filter.Description,
                    RequiredTotal = filter.RequiredTotal,
                    Skip = filter.Skip,
                    Take = filter.Take
                });

            return new ObjectResult(trees);
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

    [HttpGet]
    [Route("/Active-Biomes")]
    public async Task<ObjectResult> GetActiveTreeBiomes([FromQuery] ActiveBiomesSearchInput filter)
    {
        logger.LogInformation("Getting active tree biomes by filter");

        try
        {
            var trees = await getActiveTreeBiomesUseCase
                .Run(new GetActiveTreeBiomesUseCaseInput
                {
                    Name = filter.Name,
                    RequiredTotal = filter.RequiredTotal,
                    Skip = filter.Skip,
                    Take = filter.Take
                });

            return new ObjectResult(trees);
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
