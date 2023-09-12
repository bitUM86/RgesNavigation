using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviLib.MyTypes;
using RgesNaviApi.DataBaseContext;

namespace RgesNaviApi.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UserManagementController : Controller
    {
        private readonly ILogger<EnergyObjectController> _logger;
        ApplicationContext db;
        public UserManagementController(ILogger<EnergyObjectController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        [Route("users")]
        public IResult GetUsers() => Results.Ok(db.Users.ToList());

        [HttpGet]
        [Route("users/{id}")]
        public IResult GetUser(int id)
        {
            User? user = db.Users.FirstOrDefault(op => op.Id == id);
            return user != null ? Results.Ok(user) : Results.NotFound();
        }

        [HttpPost]
        [Route("users/add")]
        public IResult AddUser(User user)
        {
            User us = db.Users.First(u => u.Login == user.Login);
            if (us != null) return Results.Forbid();
            else
            {
                var entry = db.Users.Add(user);
                db.SaveChanges();
                return Results.Ok(entry.Entity);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("users/edit")]
        public IResult EditObject(User user)
        {
            User us = db.Users.Update(user).Entity;
            if (us == null) return Results.NotFound();
            else
            {
                db.SaveChanges();
                return Results.Ok(us);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("users/delete/{id}")]
        public JsonResult DeleteObject(int id)
        {
            User? us = db.Users.FirstOrDefault(u => u.Id == id);

            if (us != null)
            {
                db.Users.Remove(us);
                db.SaveChanges();
                return Json(us);
            }
            else return Json(null);
        }
    }
}
