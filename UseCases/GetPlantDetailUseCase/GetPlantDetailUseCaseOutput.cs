using System;
namespace Ipe.UseCases.GetPlantDetailUseCase
{
    public class GetPlantDetailUseCaseOutput
    {
        public string PlantId { get; set; }
        public string OrderId { get; set; }
        public string TreeId { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
        public List<string> Hastags { get; set; }
        public string Biome { get; set; }
        public string Species { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool CanEdit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

