using CarParkManagement.Enums;
using CarParkManagement.Models;
using CarParkManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarParkManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class ParkingController(IParkingService parkingService) : ControllerBase
{
    readonly IParkingService _parkingService = parkingService;

    [HttpPost]
    public async Task<ActionResult<ParkVehicleResponse>> ParkVehicle([FromBody] ParkVehicleRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.VehicleReg))
        {
            ModelState.AddModelError(nameof(request.VehicleReg), "VehicleReg is required.");
        }

        if (!Enum.IsDefined(typeof(VehicleType), request.VehicleType))
        {
            ModelState.AddModelError(nameof(request.VehicleType), "VehicleType must be 1, 2 or 3.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vehicleType = (VehicleType)request.VehicleType;

        var parkingDetails = await _parkingService.ParkVehicle(request.VehicleReg, vehicleType);

        return Ok(parkingDetails);
    }

    [HttpPost("exit")]
    public async Task<ActionResult<VehicleExitResponse>> EvaluateParking([FromBody] ExitParkingRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.VehicleReg))
        {
            return BadRequest();
        }

        var exitDetails = await _parkingService.ExitCarPark(request.VehicleReg);

        return Ok(exitDetails);
    }

    [HttpGet]
    public async Task<ActionResult<CarParkSummaryResponse>> GetCarParkSummary()
    {
        var carParkSummary = await _parkingService.GetCarParkState();

        return Ok(carParkSummary);
    }
}
