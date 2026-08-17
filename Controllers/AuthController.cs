using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Student_last_version.models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Student_last_version.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            
            if (model.UserName == "admin" && model.Password == "123")
            {
                


                //get the security key and then make it to byets 
                var secretKey = _configuration["Jwt:Key"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                

                // definig the algorithm 
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // make the token 
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(1), // التذكرة صالحة لمدة ساعة
                    signingCredentials: credentials);

                // generate the token string and send it to user 
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenString });
            }

           
            return Unauthorized("اسم المستخدم أو كلمة المرور غير صحيحة.");








        }
    }
}
