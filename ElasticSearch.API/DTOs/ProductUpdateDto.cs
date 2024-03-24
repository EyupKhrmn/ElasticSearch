namespace ElasticSearch.API.DTOs;

public sealed record ProductUpdateDto(
    string Id,
    string Name,
    decimal Price,
    int Stock,
    ProductFeatureDto ProductFeature);