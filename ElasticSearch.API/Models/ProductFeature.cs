using ElasticSearch.API.Models.Enums;

namespace ElasticSearch.API.Models;

public class ProductFeature
{
    public int Width { get; set; }
    public int Height { get; set; }
    public EColor Color { get; set; }
}