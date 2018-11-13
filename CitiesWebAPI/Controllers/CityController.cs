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

    public class CityController : ControllerBase
    {
        private readonly DataContext _db;
        public CityController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<DataContext> GetAll(bool ShouldGetPointsOfInterest = false)
        {
            List<Cities> city = _db.cities.ToList();
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_db.cities.Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult (_db.cities.ToList());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id, bool ShouldGetPointsOfInterest = false)
        {
            List<Cities> city = _db.cities.ToList();
            if (!city.Exists(x => x.Id == id))
            {
                return NotFound();
            }
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_db.cities.Where(x => x.Id == id));
            }
            return new ObjectResult(_db.cities.Where(x => x.Id == id).Include(x => x.pointOfInterests));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cities cities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _db.cities.Add(cities);
            _db.SaveChanges();
            return CreatedAtAction("GetAll", cities);
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody]Cities cities)
        {
            List<Cities> city = _db.cities.ToList();
            if (!city.Exists(x => x.Id == cities.Id))
            {
                return NotFound();
            }
            Cities lastCities = _db.cities.FirstOrDefault(x => x.Id == cities.Id);
            _db.Entry(lastCities).CurrentValues.SetValues(cities);
            return Ok();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public IActionResult Patch(JsonPatchDocument<Cities> cityPatch, [FromRoute]int id)
        {
            var oldCity = _db.cities.FirstOrDefault(x => x.Id == id);
            if (oldCity is null)
            {
                return NotFound();
            }
            cityPatch.ApplyTo(oldCity);
            _db.Update(oldCity);
            _db.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route ("Delete/{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            //Slette bestemte cities
            _db.cities.Remove(_db.cities.FirstOrDefault(x => x.Id == id));
            _db.SaveChanges();
            return Ok();
        }
    }
}
