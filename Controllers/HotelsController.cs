using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;

using HotelListing.Api.Application.DTOs.Hotel;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController(IHotelsService hotelsService) : BaseApiController
{


    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
    {
        var hotels = await hotelsService.GetAllHotels();
        return ToActionResult(hotels);
    }


    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetHotelDto>> GetHotel(int id)
    {
        var hotel = await hotelsService.GetHotelById(id);
        return ToActionResult(hotel);
    }
   


    // GET: api/Hotels/5
    

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto hotelDTo)
    {
        var result = await hotelsService.UpdateHotel(id, hotelDTo); 

        return ToActionResult(result);
    }

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
    {
        var resultDto = await hotelsService.CreateHotel(hotelDto);

        return CreatedAtAction("GetHotel", new { id = resultDto.Value!.Id }, resultDto.Value);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
       var result = await hotelsService.DeleteHotel(id);
        return ToActionResult(result);
    }


}



