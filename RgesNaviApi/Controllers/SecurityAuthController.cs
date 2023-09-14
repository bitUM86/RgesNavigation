using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NaviLib.DTO;
using RgesNaviApi.DataBaseContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using NaviLib.MyTypes;

namespace RgesNaviApi.Controllers
{
    [Route("login")]
    [ApiController]
    public class SecurityAuthController : Controller
    {
        ApplicationContext db;
        public SecurityAuthController(ApplicationContext context)
        {
            db = context;
        }

        [HttpPost]
        [Route("getjwt")]
                public IResult CreateJwt(LoginDto user)
        {
            var userFromDb = db.Users.FirstOrDefault(p => p.Login == user.Login &&  p.Password == user.Password);
            if (userFromDb is null) return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userFromDb.Login),
                                            new Claim(ClaimTypes.Role, userFromDb.Role)};
            
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(12)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
           
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Results.Json(new ResponseDto( userFromDb.Role, userFromDb.Username, encodedJwt ));
        }
    }
}
