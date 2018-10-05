using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;


namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        public CelestialObjectController(ApplicationDbContext app)
        {
            _context = app;
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var c in celestialObjects)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }
           
            return Ok(celestialObjects);

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.CelestialObjects.ToList();
            foreach(var c in list)
            {
                c.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == c.Id).ToList();
            }
            return Ok(list);
        }
        private readonly ApplicationDbContext _context;
    }
}
