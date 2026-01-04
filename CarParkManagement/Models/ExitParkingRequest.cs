namespace CarParkManagement.Models;

public record ExitParkingRequest
{
    public string VehicleReg { get; init; } = string.Empty;
}
