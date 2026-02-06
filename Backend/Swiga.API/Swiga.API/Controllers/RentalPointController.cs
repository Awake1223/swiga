using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swiga.API.Contracts;
using Swiga.Application.Services;
using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;

namespace Swiga.API.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class RentalPointController : ControllerBase
    {
        public readonly IRentalPointService _rentalPoint;
        public RentalPointController(IRentalPointService rentalPoint)
        {
            _rentalPoint = rentalPoint;
        }
        [HttpGet]
        public async Task<ActionResult<List<RentalPointResponse>>> GetRentalPoint()
        {
            var rentalPoints = await _rentalPoint.GetRentalPoint();

            var response = rentalPoints.Select(r => new RentalPointResponse(r.Id, r.Name, r.Address, r.City, r.PhoneNumber, r.Email));

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateRentalPoint([FromBody] RentalPointRequest request)
        {
            // Проверки полей
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest("Name is required");
            if (string.IsNullOrEmpty(request.Address))
                return BadRequest("Address is required");
            if (string.IsNullOrEmpty(request.City))
                return BadRequest("City is required");
            if (string.IsNullOrEmpty(request.PhoneNumber))
                return BadRequest("PhoneNumber is required");
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email is required");

            // Генерируем ID здесь
            var (rentalPoint, error) = RentalPointModel.Create(
                Guid.NewGuid(),  // ← ID генерируется на сервере
                request.Name,
                request.Address,
                request.City,
                request.PhoneNumber,
                request.Email);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var rentalPointId = await _rentalPoint.CreateRentalPoint(rentalPoint);
            return Ok(rentalPointId);
        }
        

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateRentalPoint(Guid id, [FromBody] RentalPointRequest request)
        {
            var rentalPointId = await _rentalPoint.UpdateRentalPoint(id, request.Name, request.Address, request.City, request.PhoneNumber, request.Email);

            return Ok(rentalPointId); 
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteRentalPoint(Guid id)
        {
            return Ok(await _rentalPoint.DeleteRentalPoint(id));
        }
    }
}
