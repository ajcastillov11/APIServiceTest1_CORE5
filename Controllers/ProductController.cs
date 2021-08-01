using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServiceAPITest.Data;
using WebServiceAPITest.Models;

namespace WebServiceAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets the products.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ModelProduct>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                List<ModelProduct> _listProducts = await _db.Products.OrderBy(x => x.ProductName)
                    .Include(x => x.Category)
                    .ToListAsync();

                return Ok(_listProducts);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(200, Type = typeof(ModelProduct))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                ModelProduct obj = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(x => x.ProductId == id);
                return obj == null ? NotFound() : Ok(obj) ;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddProduct([FromBody] ModelProduct product)
        {
            try
            {
                if (!ModelState.IsValid || product == null)
                {
                    return BadRequest(ModelState);
                }

                await _db.AddAsync(product);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
