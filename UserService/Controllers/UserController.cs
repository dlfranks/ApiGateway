using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserService.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers
{
    [Route("api/user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        

        // GET: api/<UserController>

        [HttpGet("getUser")]
        public ActionResult<IEnumerable<User>> Get()
        {
            User user = GetDummyData();
            return Ok(user);
        }

        private User GetDummyData()
        {
            User user = new User()
            {
                Id = 1,
                Name = "Deana",
                Email = "deoksoonf@gmail.com"
            };
            return user;
        }
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Get(string name , string password)
        {
            //just hard code here
            if(name == "cp" && password == "123")
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer),
                }; 
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret to add some default jwt token lets see how it works"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = "chandra",
                    ValidateAudience = true,
                    ValidAudience = "enduser",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                };

                var jwt = new JwtSecurityToken(
                        issuer:"chandra",
                        audience:"enduser",
                        claims:claims,
                        notBefore:now,
                        expires:now.Add(TimeSpan.FromMinutes(2)),
                        signingCredentials:new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                    );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var result = new
                {
                    access_token = encodedJwt.ToString(),
                    expires_in = TimeSpan.FromMinutes(2000).TotalSeconds.ToString()
                };
                return Ok(result);
            }
            return NotFound();
        }
        [HttpGet("Search")]
        public IActionResult Search(string namelike)
        {
            var result = new { aaa = "aaaa", vvv = "" };
            
            return Ok(result);
        }
        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
