using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
            [Route ("POI/{id}")]
            public ActionResult<List<Cities>> GetAllPoi(int id)
            {
                 return new ObjectResult(_db.cities.FirstOrDefault(x => x.Id == id).pointOfInterests);
            }

            [HttpGet]
            [Route("POI/{cityId}/{poiId}")]
            public IActionResult Get(int cityId, int poiId)
            {
            if (_db.cities.Where(x => x.Id == cityId).Count() == 0 || !_db.cities.FirstOrDefault(x => x.Id == cityId).pointOfInterests.Exists(x => x.Id == poiId))
            {
                return NotFound();
            }
            return new OkObjectResult(_db.cities.FirstOrDefault(x => x.Id == cityId).pointOfInterests.FirstOrDefault(x => x.Id == poiId));
            }

            [HttpPost]
            public IActionResult PostPoi([FromBody] int id, PointOfInterest poi)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _db.SaveChanges();
                _db.cities.FirstOrDefault(x => x.Id == id).pointOfInterests.Add(poi);
                return CreatedAtAction("GetAllPoi", poi);
            }

            [HttpPut]
            public IActionResult Update(int id, PointOfInterest poi)
            {
                if (_db.cities.Where(x => x.Id == id).Count() == 0)
                {
                    return NotFound();
                }
                Cities cities = _db.cities.FirstOrDefault(x => x.Id == id);
                PointOfInterest lastPoi = cities.pointOfInterests.FirstOrDefault(x => x.Id == poi.Id);
                lastPoi = poi;
                return Ok();
            }

            [HttpDelete]
            [Route("Delete/{id}")]
            public IActionResult Delete(int cityId, int poiId)
            {
            _db.cities.FirstOrDefault(x => x.Id == cityId).pointOfInterests.RemoveAll(x => x.Id == poiId);
                return Ok();
            }
        
    }

}

