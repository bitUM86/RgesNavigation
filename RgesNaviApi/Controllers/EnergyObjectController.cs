using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationContext _db;
        public EnergyObjectController( ApplicationContext context)
        {
            _db = context;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<IResult> GetObjectsByFiltering(FilterDto filterDto)
        {
            return Results.Json(await _db.EnergyObjects.Where(eo => filterDto.Name == string.Empty || eo.Name.Contains(filterDto.Name))
                                         .Where(eo => filterDto.Filter.Count == 0 || filterDto.Filter.Contains(eo.EnergyObjectType))
                                         .Where(eo => filterDto.District.Count == 0 || filterDto.District.Contains(eo.District))
                                         .ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("add")]
        public async Task<IResult> AddObject(EnergyObject energyObject)
        {
            var eObject = _db.EnergyObjects.Where(eo => eo.IsImhoEqual(energyObject));
            if (eObject == null)
            {
                var entry = _db.EnergyObjects.Add(energyObject);
                await _db.SaveChangesAsync();
                return Results.Ok(entry.Entity);
            }
            else return Results.Forbid();
        }

        [HttpGet]
        [Authorize(Roles = "admin,editor")]
        [Route("{id}")]
        public async Task<IResult> EditObject(int id)
        {
            var energyObject =await _db.EnergyObjects.FirstOrDefaultAsync(eo => eo.Id == id);
            return energyObject != null ? Results.Ok(energyObject) : Results.NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("edit")]
        public async Task<IResult> EditObject(EnergyObject eo)
        {
            var energyObject = _db.EnergyObjects.Update(eo).Entity;
            if (energyObject == null) return Results.NotFound();
            await _db.SaveChangesAsync();
            return Results.Ok(energyObject);
        }

        [HttpPost]
        [Authorize(Roles = "admin,editor")]
        [Route("delete/{id}")]
        public async Task<IResult> DeleteObject(int id)
        {
            var energyObject =await _db.EnergyObjects.FirstOrDefaultAsync(op => op.Id == id);

            if (energyObject != null)
            {
                _db.EnergyObjects.Remove(energyObject);
                await _db.SaveChangesAsync();
                return Results.Ok("Объект удален :)");
            }
            else return Results.NotFound("Объект не найден :(");
        }
    }
}
