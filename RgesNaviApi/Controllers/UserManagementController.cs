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
        public JsonResult GetUsers() => Json(db.Users.ToList());

        [HttpGet]
        [Route("users/{id}")]
        public JsonResult GetUser(int id)
        {
            User? user = db.Users.FirstOrDefault(op => op.Id == id);
            return user != null ? Json(user) : Json(null);
        }

        [HttpPost]
        [Route("users/add")]
        public string AddUser(User user)
        {
            User? us = db.Users.First(u => u.Login == user.Login);
            if (us != null) return $"Объект с ником {user.Login} уже существует";

            db.Users.Add(user);
            db.SaveChanges();
            return $"{user.Login}  добавлен...";
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("users/edit")]
        public string EditObject(User user)
        {
            User us = db.Users.Update(user).Entity;
            if (us == null) return "Пользователь не найден";
            db.SaveChanges();
            return $"Изменения пользователя {us.Login} сохранены.";
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
