using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.WebApplication1.Data;

namespace WebApplication1.WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private static readonly List<Hotel1> Hotels = new List<Hotel1>
        {
            new pHotel1 { Id = 1, Name = "Hotel A", Address = "123 Main St", Rating = 4.5 },
            new Hotel1 { Id = 2, Name = "Hotel B", Address = "456 Elm St", Rating = 3.8 },
            new Hotel1 { Id = 3, Name = "Hotel C", Address = "789 Oak St", Rating = 4.2 }
        };
        // GET: api/<HotelsController>
        [HttpGet]
        public ActionResult<IEnumerable<Hotel1>> Get()
        {
            return Ok(Hotels);
        }

        // GET api/<HotelsController>/5
        [HttpGet("{id}")]
        public ActionResult<Hotel1> Get(int id)
        {
            var hotel = Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        // POST api/<HotelsController>
        [HttpPost]
        public ActionResult<Hotel1> Post([FromBody] Hotel1 newHotel)
        {
            if (Hotels.Any(h => h.Id == newHotel.Id))
            {
                return BadRequest("Hotel with the same ID already exists.");
            }
            Hotels.Add(newHotel);
            return CreatedAtAction(nameof(Get), new { id = newHotel.Id }, newHotel);
        }

        // PUT api/<HotelsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Hotel1 updatedHotel)
        {
            var existingHotel = Hotels.FirstOrDefault(h => h.Id == id);
            if (existingHotel == null)
            {
                return NotFound();
            }
            existingHotel.Name = updatedHotel.Name;
            existingHotel.Address = updatedHotel.Address;
            existingHotel.Rating = updatedHotel.Rating;
            return Ok(existingHotel);
        }

        // DELETE api/<HotelsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var hotel = Hotels.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound(new{ message = "Hotel not found." });
            }
            Hotels.Remove(hotel);
            return NoContent();
        }
    }
}
