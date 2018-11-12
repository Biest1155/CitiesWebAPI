using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_db.cities.Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult (_db.cities.ToList());
        }

        [HttpGet]
        [Route("City/{id}")]
        public IActionResult Get(int id, bool ShouldGetPointsOfInterest = false)
        {
            if (_db.cities.Where(x => x.Id == id).Count() == 0)
            {
                return NotFound();
            }
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_db.cities.Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult(_db.cities.FirstOrDefault(x => x.Id == id));
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
        public IActionResult Update(Cities cities)
        {
            if (_db.cities.Where(x => x.Id == cities.Id).Count() == 0)
            {
                return NotFound();
            }
            Cities lastCities = _db.cities.FirstOrDefault(x => x.Id == cities.Id);
            lastCities = cities;
            return Ok();
        }

        [HttpDelete]
        [Route ("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            //Slette bestemte cities
            _db.cities.Remove(_db.cities.FirstOrDefault(x => x.Id == id));
            return Ok();
        }
    }
}
