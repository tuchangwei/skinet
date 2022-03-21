using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;

namespace API.Controllers
{
    // [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;

        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandsRepo,
        IGenericRepository<ProductType> productTypesRepo)
        {
            _productsRepo = productsRepo;
            _productBrandsRepo = productBrandsRepo;
            _productTypesRepo = productTypesRepo;
        }

        [HttpGet] 
       public async Task<ActionResult<List<Product>>> GetProducts() {
           var spec = new ProductsWithTypesAndBrandsSpecification();
           var products = await _productsRepo.ListAsync(spec); 
           return Ok(products); 
       }
       [HttpGet("{id}")]
       public async Task<ActionResult<Product>> GetProduct(int id) {
           return Ok(await _productsRepo.ListAllAsync()); 
       }

        [HttpGet("brands")] 
       public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands() {
           var brands = await _productBrandsRepo.ListAllAsync();
           return Ok(brands); 
       }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() {
           var types = await _productTypesRepo.ListAllAsync(); 
           return Ok(types); 
       }
    }
}