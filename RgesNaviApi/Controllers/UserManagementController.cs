using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaviLib.MyTypes;
using RgesNaviApi.DataBaseContext;

namespace RgesNaviApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationContext _db;
        public UserManagementController(ApplicationContext context)
        {
            _db = context;
        }

        [HttpGet]
        [Route("")]
        public async Task<IResult> GetUsers() => Results.Ok(await _db.Users.ToListAsync());

        [HttpGet]
        [Route("{id}")]
        public async Task<IResult> GetUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(op => op.Id == id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }

        [HttpPost]
        [Route("add")]
        public async Task<IResult> AddUser(User user)
        {
            var us = await _db.Users.FirstAsync(u => u.Login == user.Login);
            if (us != null) return Results.Forbid();
            else
            {
                var entry = _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return Results.Ok(entry.Entity);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("edit")]
        public async Task<IResult> EditObject(User user)
        {
            var us = _db.Users.Update(user).Entity;
            if (us == null) return Results.NotFound();
            else
            {
                await _db.SaveChangesAsync();
                return Results.Ok(us);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("delete/{id}")]
        public async Task<IResult> DeleteObject(int id)
        {
            var us = _db.Users.FirstOrDefault(u => u.Id == id);

            if (us != null)
            {
                _db.Users.Remove(us);
                await _db.SaveChangesAsync();
                return Results.Ok(us);
            }
            else return Results.NotFound();
        }
    }
}
