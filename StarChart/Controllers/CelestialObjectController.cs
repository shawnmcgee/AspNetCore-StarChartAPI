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
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if(celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == id).ToList();
            return Ok(celestialObject);
        }
           
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjectName = _context.CelestialObjects.Where(e => e.Name == name);
            if (celestialObjectName == null)
                return NotFound();
            foreach (var celestialObject in celestialObjectName)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == celestialObject.Id).ToList();
            }           
            return Ok(celestialObjectName);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCelestialObjects = _context.CelestialObjects.ToList();
            foreach(var celestialObject in allCelestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == celestialObject.Id).ToList();
            }
            return Ok(allCelestialObjects);
        }
    }
}
