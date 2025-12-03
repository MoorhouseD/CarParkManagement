using CarParkManagement.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CarParkManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class ParkingController : ControllerBase
{
    [HttpPost]
    public async Task<string> RegisterVehicleParking(string vehicleReg, VehicleType vehicleType)
    {

    }

    [HttpPost("exit")]
    public async Task<string> EvaluateParking(string vehicleReg)
    {

    }

    [HttpGet]
    public async Task GetCarParkSummary()
    {

    }
}
