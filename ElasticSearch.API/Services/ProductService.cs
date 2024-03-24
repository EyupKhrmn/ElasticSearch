using System.Collections.Immutable;
using System.Net;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using ElasticSearch.API.Repositories;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.OpenApi.Extensions;
using Nest;

namespace ElasticSearch.API.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository productRepository,ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {   
        var response = await _productRepository.AddProductAsync(request.ToModel());

        if (response is null)
        {
            return ResponseDto<ProductDto>.Fail(new List<string> {"kayıt esnasında bir hata meydana geldi"},
                HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(new ProductDto(response.Id,response.Name,response.Price,response.Stock,new ProductFeatureDto(response.ProductFeature.Width,response.ProductFeature.Height,response.ProductFeature.Color.GetDisplayName())), HttpStatusCode.Created);
    }


    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        var productListDto = new List<ProductDto>();

        foreach (var pro in products)
        {
            if (pro.ProductFeature is null)
                productListDto.Add(new ProductDto(pro.Id, pro.Name, pro.Price, pro.Stock,null));
            
            productListDto.Add(new ProductDto(pro.Id, pro.Name, pro.Price, pro.Stock, new ProductFeatureDto(pro.ProductFeature.Width, pro.ProductFeature.Height, pro.ProductFeature.Color.GetDisplayName())));
        }
        
        return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var product = await _productRepository.GetById(id);

        if (product is null) 
            return ResponseDto<ProductDto>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
        
        return ResponseDto<ProductDto>.Success(product.ToDto(), HttpStatusCode.NotFound);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productUpdateDto)
    {
        var result = await _productRepository.UpdateAsync(productUpdateDto);
        
        if (!result) 
            return ResponseDto<bool>.Fail("Güncelleme yapılamadı", HttpStatusCode.InternalServerError);

        return ResponseDto<bool>.Success(result, HttpStatusCode.NoContent);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var deleteResponse = await _productRepository.DeleteAsync(id);
        
        if (!deleteResponse.IsValid && deleteResponse.Result == Result.NotFound)
        {
            return ResponseDto<bool>.Fail("Silmeye Çalıştığınız öğre bulunamadı", HttpStatusCode.NotFound);
        }

        if (!deleteResponse.IsValid)
        {
            _logger.LogError(deleteResponse.OriginalException,deleteResponse.ServerError.ToString());
            return ResponseDto<bool>.Fail("Silme İşlemi yapılamadı", HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }
}