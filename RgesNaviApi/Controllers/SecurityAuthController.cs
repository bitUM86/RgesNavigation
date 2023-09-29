using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NaviLib.DTO;
using RgesNaviApi.DataBaseContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace RgesNaviApi.Controllers;

[Route("login")]
[ApiController]
public class SecurityAuthController : Controller
{
    private readonly ApplicationContext _db;
    public SecurityAuthController(ApplicationContext context)
    {
        _db = context;
    }

    [HttpPost]
    [Route("getjwt")]
    public async Task<IResult> CreateJwt(LoginDto user)
    {
        var userFromDb =await _db.Users.FirstOrDefaultAsync(p => p.Login == user.Login && p.Password == user.Password);
        if (userFromDb is null) return Results.Unauthorized();

        var claims = new List<Claim> { new(ClaimTypes.Name, userFromDb.Login),
            new(ClaimTypes.Role, userFromDb.Role)};
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(12)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Results.Json(new ResponseDto(userFromDb.Role, userFromDb.Username, encodedJwt));
    }
}