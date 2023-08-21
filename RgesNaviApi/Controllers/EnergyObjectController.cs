using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviLib.DTO;
using NaviLib.MyTypes;
using RgesNaviApi.DataBaseContext;
using RgesNaviApi.Extensions;


namespace RgesNaviApi.Controllers
{
    [ApiController]
    [Route("api")]

    public class EnergyObjectController : Controller
    {
        private readonly ILogger<EnergyObjectController> _logger;
        ApplicationContext db;
        public EnergyObjectController(ILogger<EnergyObjectController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpPost, Authorize]
        [Route("objects/{FilterDto filter}")]
        public JsonResult GetObjectsByFiltering(FilterDto filterDto)
        {
            _logger.LogInformation($"Пришел запрос с параметрами:\r\n" +
                                   $"Имя: {filterDto.Name}\r\n" +
                                   $"Тип объекта: {String.Join(", ", filterDto.Filter)} \r\n" +
                                   $"\r\n подразделение: {String.Join(", ", filterDto.District)} ");

            return Json(db.EnergyObjects.Where(eo => filterDto.Name == String.Empty ? true : eo.Name.Contains(filterDto.Name))
                                         .Where(eo => filterDto.Filter.Count == 0 ? true : filterDto.Filter.Contains(eo.EnergyObjectType))
                                         .Where(eo => filterDto.District.Count == 0 ? true : filterDto.District.Contains(eo.District))
                                         .ToList());
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/add")]
        public string AddObject(EnergyObject energyObject)
        {
            var eObject = db.EnergyObjects.Where(eo => eo.IsImhoEqual(energyObject));
            if (eObject != null) return $"объект {energyObject.EnergyObjectType} - {energyObject.Name} уже существует!";
            try
            {
                var entity = db.EnergyObjects.Add(energyObject);
                db.SaveChanges();
                return entity.Entity.Name.ToString();
            }
            catch (Exception ex)
            {
                return $"Что то наебнулось.  {ex.Message}";
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/edit/{id}")]
        public JsonResult EditObject(int id)
        {
            EnergyObject? energyObject = db.EnergyObjects.FirstOrDefault(eo => eo.Id == id);
            return energyObject != null ? Json(energyObject) : Json(null);
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/edit")]
        public string EditObject(EnergyObject eo)
        {
            EnergyObject energyObject = db.EnergyObjects.Update(eo).Entity;
            if (energyObject == null) return "Объект не найден";

            db.SaveChanges();
            return $"Изменения объекта {energyObject.EnergyObjectType} - {energyObject.Name} сохранены.";
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/delete/{id}")]
        public string DeleteObject(int id)
        {
            EnergyObject? energyObject = db.EnergyObjects.FirstOrDefault(op => op.Id==id);

            if (energyObject != null)
            {
                db.EnergyObjects.Remove(energyObject);
                db.SaveChanges();
                return $"Объект {energyObject.EnergyObjectType} - {energyObject.Name} удален";
            }
            else return $"Объект с Id = {id} не найден";
        }
    }
}
