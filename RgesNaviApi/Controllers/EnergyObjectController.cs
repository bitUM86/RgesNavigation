using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviLib.DTO;
using NaviLib.MyTypes;
using RgesNaviApi.DataBaseContext;
using RgesNaviApi.Extensions;


namespace RgesNaviApi.Controllers
{
    [ApiController]
    [Route("api/objects")]
    public class EnergyObjectController : Controller
    {
        readonly ApplicationContext _db;
        public EnergyObjectController( ApplicationContext context)
        {
            _db = context;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public IResult GetObjectsByFiltering(FilterDto filterDto)
        {
            return Results.Json(_db.EnergyObjects.Where(eo => filterDto.Name == string.Empty ? true : eo.Name.Contains(filterDto.Name))
                                         .Where(eo => filterDto.Filter.Count == 0 ? true : filterDto.Filter.Contains(eo.EnergyObjectType))
                                         .Where(eo => filterDto.District.Count == 0 ? true : filterDto.District.Contains(eo.District))
                                         .ToList());
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("add")]
        public IResult AddObject(EnergyObject energyObject)
        {
            var eObject = _db.EnergyObjects.Where(eo => eo.IsImhoEqual(energyObject));
            if (eObject == null)
            {
                var entry = _db.EnergyObjects.Add(energyObject);
                _db.SaveChanges();
                return Results.Ok(entry.Entity);
            }
            else return Results.Forbid();
        }

        [HttpGet]
        [Authorize(Roles = "admin,editor")]
        [Route("{id}")]
        public IResult EditObject(int id)
        {
            var energyObject = _db.EnergyObjects.FirstOrDefault(eo => eo.Id == id);
            return energyObject != null ? Results.Ok(energyObject) : Results.NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("edit")]
        public IResult EditObject(EnergyObject eo)
        {
            var energyObject = _db.EnergyObjects.Update(eo).Entity;
            if (energyObject == null) return Results.NotFound();
            _db.SaveChanges();
            return Results.Ok(energyObject);
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("delete/{id}")]
        public IResult DeleteObject(int id)
        {
            var energyObject = _db.EnergyObjects.FirstOrDefault(op => op.Id == id);

            if (energyObject != null)
            {
                _db.EnergyObjects.Remove(energyObject);
                _db.SaveChanges();
                return Results.Ok();
            }
            else return Results.NotFound();
        }
    }
}
