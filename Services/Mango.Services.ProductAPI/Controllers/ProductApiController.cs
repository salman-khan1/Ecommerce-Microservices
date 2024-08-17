using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductApiController : ControllerBase
{

    private readonly AppDbContext dbContext;
    private ResponseDto _responseDto;
    IMapper _mapper;

    public ProductApiController(AppDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        _responseDto = new ResponseDto();
        _mapper = mapper;
    }


    [HttpGet]
    public ResponseDto GetProducts()
    {
        try
        {
            IEnumerable<Product> productList = dbContext.Products.ToList();
            _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
        }
        catch (Exception ex) {
            _responseDto.Success = false;
            _responseDto.Message=ex.Message;
        }
        return _responseDto;

    }


    [HttpGet("{id}")]
    public ResponseDto GetProductById(int id)
    {
        try
        {
            Product obj = dbContext.Products.First(x => x.ProductId == id);
          
            _responseDto.Result = _mapper.Map<Product>(obj);

        }
        catch (Exception ex)
        {

            _responseDto.Success = false;
            _responseDto.Message = ex.Message;
        }
        return _responseDto;
    }


    [HttpPost]
   // [Authorize(Roles = "ADMIN")]
    public ResponseDto PostProduct([FromBody] ProductDto ProductDto)
    {

        try
        {
            Product ProductObj = _mapper.Map<Product>(ProductDto);
            dbContext.Products.Add(ProductObj);
            dbContext.SaveChanges();
            _responseDto.Result = _mapper.Map<ProductDto>(ProductObj);
        }
        catch (Exception ex)
        {

            _responseDto.Success = false;
            _responseDto.Message = ex.Message;
        }
        return _responseDto;
    }

    [HttpPut]
    //[Authorize(Roles = "ADMIN")]
    public ResponseDto PutProduct([FromBody] ProductDto ProductDto)
    {

        try
        {
            Product ProductObj = _mapper.Map<Product>(ProductDto);
            dbContext.Products.Update(ProductObj);
            dbContext.SaveChanges();
            _responseDto.Result = _mapper.Map<ProductDto>(ProductObj);
        }
        catch (Exception ex)
        {

            _responseDto.Success = false;
            _responseDto.Message = ex.Message;
        }
        return _responseDto;
    }
    [HttpDelete("{id}")]
    //[Authorize(Roles = "ADMIN")]
    public ResponseDto DeleteProduct(int id)
    {

        try
        {
            Product ProductObj = dbContext.Products.First(x => x.ProductId == id);
            dbContext.Products.Remove(ProductObj);
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {

            _responseDto.Success = false;
            _responseDto.Message = ex.Message;
        }
        return _responseDto;
    }
}

