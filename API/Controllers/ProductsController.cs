using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]   //api/products
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var products = await _repo.GetProductsAsync(brand,type,sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")]  //api/products/2
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }


        [HttpPost]   //create endpoint
        public async Task<ActionResult<Product>> CreateProducts(Product p)
        {
            _repo.AddProduct(p);
            if (await _repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProducts", new { id = p.Id }, p);
            }
            return BadRequest("Problem Creating Product");
        }

        [HttpPut("{id:int}")]  //update
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot Update This Product");

            _repo.UpdateProduct(product);
            if (await _repo.SaveChangesAsync())
            {

                return NoContent();
            }
            return BadRequest("Problem Updating Product");
        }

        [HttpDelete("{id:int}")]  //delete
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            _repo.DeleteProduct(product);
            if (await _repo.SaveChangesAsync())
            {

                return NoContent();
            }
            return BadRequest("Problem Deleting Product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _repo.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _repo.GetTypesAsync());
        }

        private bool ProductExists(int id)
        {
            return _repo.ProductExists(id);
        }
    }
}
