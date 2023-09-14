using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public UserManagementController( ApplicationContext context)
        {
            _db = context;
        }

        [HttpGet]
        [Route("")]
        public IResult GetUsers() => Results.Ok(_db.Users.ToList());

        [HttpGet]
        [Route("{id}")]
        public IResult GetUser(int id)
        {
           var user = _db.Users.FirstOrDefault(op => op.Id == id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IResult AddUser(User user)
        {
            var us = _db.Users.First(u => u.Login == user.Login);
            if (us != null) return Results.Forbid();
            else
            {
                var entry = _db.Users.Add(user);
                _db.SaveChanges();
                return Results.Ok(entry.Entity);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("edit")]
        public IResult EditObject(User user)
        {
            var us = _db.Users.Update(user).Entity;
            if (us == null) return Results.NotFound();
            else
            {
                _db.SaveChanges();
                return Results.Ok(us);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("delete/{id}")]
        public JsonResult DeleteObject(int id)
        {
            var us = _db.Users.FirstOrDefault(u => u.Id == id);

            if (us != null)
            {
                _db.Users.Remove(us);
                _db.SaveChanges();
                return Json(us);
            }
            else return Json(null);
        }
    }
}
