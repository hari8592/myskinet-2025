using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _repo;
        public ProductsController(IGenericRepository<Product> repo)
        {
            _repo = repo;
        }

        [HttpGet]   //api/products
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            //use specification
            var spec = new ProductFilter_Sort_Pagination_Specification(specParams);


            return await CreatePagedResult(_repo, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]  //api/products/2
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }


        [HttpPost]   //create endpoint
        public async Task<ActionResult<Product>> CreateProducts(Product p)
        {
            _repo.Add(p);
            if (await _repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProducts", new { id = p.Id }, p);
            }
            return BadRequest("Problem Creating Product");
        }

        [HttpPut("{id:int}")]  //update
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot Update This Product");

            _repo.Update(product);
            if (await _repo.SaveAllAsync())
            {

                return NoContent();
            }
            return BadRequest("Problem Updating Product");
        }

        [HttpDelete("{id:int}")]  //delete
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            _repo.Remove(product);
            if (await _repo.SaveAllAsync())
            {

                return NoContent();
            }
            return BadRequest("Problem Deleting Product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            var brands = await _repo.ListAsync(spec);
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            var types = await _repo.ListAsync(spec);
            return Ok(types);
        }

        private bool ProductExists(int id)
        {
            return _repo.Exists(id);
        }
    }
}
