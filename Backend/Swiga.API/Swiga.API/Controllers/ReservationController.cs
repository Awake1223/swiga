using Microsoft.AspNetCore.Mvc;
using Swiga.API.Contracts;
using Swiga.API.Contracts.Reservation;
using Swiga.Application.DTOs;
using Swiga.Application.DTOs.Reservations;
using Swiga.Application.Services;
using Swiga.Domain.Models;

namespace Swiga.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(CreateReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateReservationResponse>> CreateReservation([FromBody] CreateReservationRequest request)
        {
            var dto = new CreateReservationDto
            {
                ClientId = request.ClientId,
                RentalPointId = request.RentalPointId,
                StartDate = request.StartDate.ToUniversalTime(),
                EndDate = request.EndDate.ToUniversalTime(),
                Items = request.Items.Select(i => new ReservationItemDto
                {
                    InventoryId = i.InventoryId,
                    Quantity = i.Quantity,
                    PricePerHour = i.PricePerHour
                }).ToList()
            };

            var (reservationId, error) = await _reservationService.CreateReservationAsync(dto);

            if (!string.IsNullOrEmpty(error)) 
                return BadRequest(new { error });

            return Ok(new CreateReservationResponse
            {
                ReservationId = reservationId.Value,
                Message = "Бронирование успешно",
                CreatedAt = DateTime.UtcNow,
            });
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReservationResponse>> GetReservation(Guid id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);

            if (reservation == null) 
                return NotFound();

            return Ok(MapToResponse(reservation));
        }


        [HttpGet("client/{clientId:guid}")]
        [ProducesResponseType(typeof(List<ReservationResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ReservationResponse>>> GetClientReservations(Guid clientId)
        {
            var reservations = await _reservationService.GetClientReservationsAsync(clientId);

            return Ok(reservations.Select(MapToResponse).ToList());
        }



        [HttpGet("available/{rentalPointId:guid}")]
        [ProducesResponseType(typeof(List<AvailableInventoryResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AvailableInventoryResponse>>> GetAvailableInventory(
            Guid rentalPointId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var available = await _reservationService.GetAvailableInventoryAsync(rentalPointId, startDate, endDate);

            return Ok(available.Select(a => new AvailableInventoryResponse
            {
                InventoryId = a.InventoryId,
                Name = a.Name,
                Size = a.Size,
                Gender = a.Gender.ToString(),
                PricePerHour = a.PricePerHour,
                AvailableQuantity = a.AvailableQuantity,
                TotalQuantity = a.TotalQuantity,
            }).ToList());
        }

        [HttpPut("{id:guid}/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmReservation(Guid id)
        {
            var (success, error) = await _reservationService.ConfirmReservationAsync(id);

            if (!success)
                return BadRequest(new { error });

            return Ok(new { message = "Бронирование подтверждено" });
        }

        [HttpPut("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelReservation(Guid id, [FromBody] CancelReservationRequest request)
        {
            var (success, error) = await _reservationService.CancelReservationAsync(id, request.Reason);

            if (!success)
                return BadRequest(new { error });

            return Ok(new { message = "Бронирование отменено" });
        }


        private ReservationResponse MapToResponse(ReservationModel reservation)
        {
            return new ReservationResponse
            {
                Id = reservation.Id,
                ClientId = reservation.ClientId,
                RentalPointId = reservation.RentalPointId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Status = reservation.Status.ToString(),
                TotalPrice = reservation.TotalPrice,
                CreatedAt = reservation.CreatedAt,
                ConfirmedAt = reservation.ConfirmedAt,
                CancelledAt = reservation.CancelledAt,
                CancellationReason = reservation.CancellationReason,
                Items = reservation.Items?.Select(i => new ReservationItemResponse
                {
                    InventoryId = i.InventoryId,
                    InventoryName = i.Inventory?.Name ?? "Неизвестно",
                    Size = i.Size,
                    Gender = i.Gender.ToString(),
                    Quantity = i.Quantity,
                    PricePerHour = i.PricePerHour,
                    TotalPrice = i.TotalPrice
                }).ToList() ?? new List<ReservationItemResponse>()
            };
        }


    }



}
