using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebServiceAPITest.Data;
using WebServiceAPITest.Models;

namespace WebServiceAPITest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly AppDbContext _db;

        public LoginController(IConfiguration config, AppDbContext db)
        {
            _config = config;
            _db = db;
        }


        /// <summary>
        /// Logins the specified user login.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public IActionResult Login([FromBody] ModelUser userLogin)
        {
            try
            {
                ModelUserAuth modelResult = new();
                IActionResult response = Unauthorized();
                ModelUser User = AuthenticateUser(userLogin);

                if (User != null)
                {
                    modelResult.UserName = userLogin.UserName;
                    modelResult.BearerToken = GenerateJSONWebToken(User);
                    modelResult.IsAuthenticated = true;

                    response = Ok(modelResult);
                } else
                {
                    return StatusCode(404, "Invalid User Name/Password.");
                }

                return response;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private string GenerateJSONWebToken(ModelUser userInfo)
        {
            try
            {
                ModelUserAuth modelResult = new ModelUserAuth();
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                Claim[] claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName,userInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Iss,_config["Jwt:Issuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud,_config["Jwt:Issuer"]),
                    new Claim("IsAuthenticated","true"),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    claims:claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:minutesToExpiration"])),
                    signingCredentials: credentials);

                string encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
                
                return encodeToken;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="userLogin">The user login.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ModelUser AuthenticateUser(ModelUser userLogin)
        {
            try
            {
                ModelUser model = _db.Users.FirstOrDefault(x => x.UserName == userLogin.UserName && x.Password == userLogin.Password);
                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
