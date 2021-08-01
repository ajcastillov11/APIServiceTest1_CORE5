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
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoriaController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ModelCategory>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                List<ModelCategory> _list = await _db.Category.OrderBy(x => x.CategoryName).ToListAsync();
                return Ok(_list);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id:int}",Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(ModelCategory))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                ModelCategory obj = await _db.Category.FirstOrDefaultAsync(x => x.CategoryId == id);
                return obj == null ? NotFound() : Ok(obj);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> addCategory([FromBody] ModelCategory category)
        {
            try
            {
                if (category == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _db.AddAsync(category);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetCategory", new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
