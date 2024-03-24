using ElasticSearch.API.Models;
using ElasticSearch.API.Models.Enums;
using Microsoft.OpenApi.Extensions;

namespace ElasticSearch.API.DTOs;

public sealed record ProductCreateDto(
    string Name,
    decimal Price,
    int Stock,
    ProductFeatureDto ProductFeature)
{
    public Product ToModel() => new()
    {
        Name = Name,
        Price = Price,
        Stock = Stock,
        ProductFeature = new ProductFeature() {Height = ProductFeature.Height, Width = ProductFeature.Width, Color = (EColor)int.Parse(ProductFeature.Color)}
    };
}