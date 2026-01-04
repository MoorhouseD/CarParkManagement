namespace CarParkManagement.Models;

public record ParkVehicleRequest
{
    public string VehicleReg { get; init; } = string.Empty;
    public int VehicleType { get; init; }
}
