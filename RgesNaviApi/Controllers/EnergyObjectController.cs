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

        [HttpPost]
        [Authorize]
        [Route("objects")]
        public IResult GetObjectsByFiltering(FilterDto filterDto)
        {
            return Results.Json(db.EnergyObjects.Where(eo => filterDto.Name == String.Empty ? true : eo.Name.Contains(filterDto.Name))
                                         .Where(eo => filterDto.Filter.Count == 0 ? true : filterDto.Filter.Contains(eo.EnergyObjectType))
                                         .Where(eo => filterDto.District.Count == 0 ? true : filterDto.District.Contains(eo.District))
                                         .ToList());
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/add")]
        public IResult AddObject(EnergyObject energyObject)
        {
            var eObject = db.EnergyObjects.Where(eo => eo.IsImhoEqual(energyObject));
            if (eObject == null)
            {
                var entry = db.EnergyObjects.Add(energyObject);
                db.SaveChanges();
                return Results.Ok(entry.Entity);
            }
            else return Results.Forbid();
        }

        [HttpGet]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/{id}")]
        public IResult EditObject(int id)
        {
            EnergyObject? energyObject = db.EnergyObjects.FirstOrDefault(eo => eo.Id == id);
            return energyObject != null ? Results.Ok(energyObject) : Results.NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/edit")]
        public IResult EditObject(EnergyObject eo)
        {
            EnergyObject energyObject = db.EnergyObjects.Update(eo).Entity;
            if (energyObject == null) return Results.NotFound();
            db.SaveChanges();
            return Results.Ok(energyObject);
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("objects/delete/{id}")]
        public IResult DeleteObject(int id)
        {
            EnergyObject? energyObject = db.EnergyObjects.FirstOrDefault(op => op.Id == id);

            if (energyObject != null)
            {
                db.EnergyObjects.Remove(energyObject);
                db.SaveChanges();
                return Results.Ok();
            }
            else return Results.NotFound();
        }
    }
}
