using CarParkManagement.Enums;

namespace CarParkManagement.Models;

public class Vehicle
{
    public required string VehicleReg { get; set; }
    public VehicleType VehicleType { get; set; }
}
