using System.Collections.Immutable;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using Nest;

namespace ElasticSearch.API.Repositories;

public class ProductRepository
{
    private readonly ElasticClient _client;
    private const string productIndex = "products";


    public ProductRepository(ElasticClient client)
    {
        _client = client;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        product.Created = DateTime.Now;
        
        var response = await _client.IndexAsync(product, _=>_.Index(productIndex).Id(Guid.NewGuid().ToString()));

        if (!response.IsValid) return null;

        product.Id = response.Id;

        return product;
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync()
    {
        var result = await _client.SearchAsync<Product>(_=>
            _.Index(productIndex)
                .Query(_=>_.MatchAll()));

        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
        
        return result.Documents.ToImmutableList();
    }

    public async Task<Product> GetById(string id)
    {
        var result = await _client.GetAsync<Product>(id, _ => _.Index(productIndex));

        if (!result.IsValid) return null;

        result.Source.Id = result.Id; 
        
        return result.Source;
    }

    public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        var response =
            await _client.UpdateAsync<Product, ProductUpdateDto>(productUpdateDto.Id,
                _ => _.Index(productIndex).Doc(productUpdateDto));

        return response.IsValid;
    }
    
    public async Task<DeleteResponse> DeleteAsync(string id)
    {
        var response = await _client.DeleteAsync<Product>(id, _ => _.Index(productIndex));
        
        return response;
    }
}