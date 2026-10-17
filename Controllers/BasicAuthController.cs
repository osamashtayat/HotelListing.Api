using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using HotelListing.Api.Common.Constants;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 [Authorize(AuthenticationSchemes =AuthenticationDefaults.BasicScheme)]

    public class BasicAuthController(HotelListingDbContext context) : ControllerBase
    {
      

        // GET: api/ApiKey
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            return await context.Hotels.ToListAsync();
        }

        // GET: api/ApiKey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> Getid(int id)
        {
            var hotel = await context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        // PUT: api/ApiKey/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiKey
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
        {
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/ApiKey/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            context.Hotels.Remove(hotel);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return context.Hotels.Any(e => e.Id == id);
        }
    }
}
