using CarParkManagement.Enums;

namespace CarParkManagement.Models;

public class Vehicle(string vehicleReg, VehicleType vehicleType)
{
    public string VehicleReg { get; } = vehicleReg;
    public VehicleType VehicleType { get; } = vehicleType;
}
