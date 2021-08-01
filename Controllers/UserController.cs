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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UserController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ModelUser>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                List<ModelUser> _list = await _db.Users.OrderBy(x => x.UserName).ToListAsync();
                return Ok(_list);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id:int}",Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(ModelUser))]
        [ProducesResponseType(400)] //Bad Request
        [ProducesResponseType(404)] //Not Found
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                ModelUser obj = await _db.Users.FirstOrDefaultAsync(x => x.UserId == id);
                return obj == null ? NotFound() : Ok(obj);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddUser([FromBody] ModelUser user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _db.AddAsync(user);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetUser", new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
