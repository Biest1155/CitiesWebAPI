using CitiesWebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class POIController : ControllerBase
    {

        private readonly DataContext _db;
        public POIController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<Cities>> GetAllPoi([FromRoute]int id)
        {
            return new ObjectResult(_db.cities.Include(x => x.pointOfInterests).FirstOrDefault(x => x.Id == id));
        }

        [HttpGet]
        [Route("{cityId}/{poiId}")]
        public IActionResult Get([FromRoute]int cityId, [FromRoute] int poiId)
        {
            
            if (!_db.cities.ToList().Exists(x => x.Id == cityId) || !_db.cities.Include(x => x.pointOfInterests).FirstOrDefault(x => x.Id == cityId).pointOfInterests.Exists(x => x.Id == poiId))
            {
                return NotFound();
            }
            return new OkObjectResult((_db.cities.Include(x => x.pointOfInterests).FirstOrDefault(x => x.Id == cityId).pointOfInterests.FirstOrDefault(x => x.Id == poiId)));
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult PostPoi([FromRoute]int id, [FromBody] PointOfInterest poi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var findCity = _db.cities.Include(x=> x.pointOfInterests).FirstOrDefault(x => x.Id == id);
            findCity.pointOfInterests.Add(poi);
            _db.Update(findCity);
            _db.SaveChanges();
            return CreatedAtAction("GetAllPoi", poi);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute]int id, PointOfInterest poi)
        {
            var lastPoi = _db.poi.FirstOrDefault(x => x.Id == id);
            if (lastPoi is null)
            {
                return NotFound();
            }
            _db.Entry(lastPoi).CurrentValues.SetValues(poi);
            return Ok();
        }

        [HttpPatch]
        [Route ("Update/{cityId}/{poiId}")]
        public IActionResult Patch (JsonPatchDocument<PointOfInterest> poiPatch, [FromRoute]int cityId, [FromRoute]int poiId)
        {
            var oldPoi = _db.cities.Include(x => x.pointOfInterests).FirstOrDefault(x => x.Id == cityId).pointOfInterests.FirstOrDefault(x => x.Id == poiId);
            if (oldPoi is null)
            {
                return NotFound();
            }
            poiPatch.ApplyTo(oldPoi);
            _db.Update(oldPoi);
            _db.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("Delete/{cityId}/{poiId}")]
        public IActionResult Delete([FromRoute]int cityId, [FromRoute] int poiId)
        {
            var deadpoi = _db.cities.Include(x => x.pointOfInterests).FirstOrDefault(x => x.Id == cityId).pointOfInterests.FirstOrDefault(x => x.Id == poiId);
            if (deadpoi is null)
            {
                return NotFound();
            }
            _db.poi.Remove(deadpoi);
            _db.SaveChanges();

            return Ok();
        }

    }

}

