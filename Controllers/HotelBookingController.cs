using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.AuthorizationFilters;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Hotel;
using Humanizer;
using HotelListing.Api.Application.DTOs.Booking;
using Microsoft.AspNetCore.Authorization;
using HotelListing.Api.Common.Constants;
using HotelListing.Api.Common.Models.Paging;

namespace WebApplication1.Controllers
{
    [Route("api/hotels/{hotelId:int}/bookings")]
    [ApiController]
    [Authorize]
    public class HotelBookingController(IBookingService bookingService) : BaseApiController    
    {


        // GET: api/HotelBooking
        [HttpGet("admin")]
     [HotelOrSystemAdmin]
        public async Task<ActionResult<PagedResult<GetBookingDto>>> GetBookingsAdmin([FromRoute] int hotelId,[FromQuery] PaginationParameters paginationParameters)
        {
            var result = await bookingService.GetBookingForHotelAsync(hotelId, paginationParameters);
            return ToActionResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetBookingDto>>> GetBookings([FromRoute] int hotelId, [FromQuery] PaginationParameters paginationParameters)
        {
            var result = await bookingService.GetUserBookingForHotelAsync(hotelId, paginationParameters);
            return ToActionResult(result);
        }


      

        // POST: api/HotelBooking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetBookingDto>> CreateBooking([FromRoute] int hotelId, [FromBody] CreateBookingDto bookingDto)
        {
            var result = await bookingService.CreateBookingAsync(bookingDto);
            return ToActionResult(result);
        }

        [HttpPut("{bookingId:int}")]
        public async Task<ActionResult<GetBookingDto>> UpdateBooking([FromRoute] int hotelId, [FromRoute] int bookingId, [FromBody] UpdateBookingDto bookingDto)
        {
            var result = await bookingService.UpdateBookingAsync(hotelId, bookingId, bookingDto);
            return ToActionResult(result);
        }
        [HttpPut("{bookingId:int}/cancel")]
        public async Task<ActionResult> CancelBooking([FromRoute] int hotelId, [FromRoute] int bookingId)
        {
            var result = await bookingService.CancelBookingAsync(hotelId, bookingId);
            return ToActionResult(result);
        }
        
        [Authorize(Roles = RoleNames.Admin + "," + RoleNames.HotelAdmin)]
        [HttpPut("{bookingId:int}/admin/cancel")]
        [HotelOrSystemAdmin]
        public async Task<ActionResult> CancelBookingForAdmin([FromRoute] int hotelId, [FromRoute] int bookingId)
        {
            var result = await bookingService.CancelBookingForAdminAsync(hotelId, bookingId);
            return ToActionResult(result);
        }
        
         [Authorize(Roles = RoleNames.Admin + "," + RoleNames.HotelAdmin)]
        [HttpPut("{bookingId:int}/admin/confirm")]
                public async Task<ActionResult> ConfirmBookingForAdmin([FromRoute] int hotelId, [FromRoute] int bookingId)
        {
            var result = await bookingService.ConfirmBookingForAdminAsync(hotelId, bookingId);
            return ToActionResult(result);
        }

    }
}
