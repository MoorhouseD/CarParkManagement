namespace CarParkManagement.Models;

public record VehicleExitResponse(string VehicleReg, decimal VehicleCharge, DateTime TimeIn, DateTime TimeOut);
