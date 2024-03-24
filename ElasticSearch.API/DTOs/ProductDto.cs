namespace ElasticSearch.API.DTOs;

public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? ProductFeature)
{
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}