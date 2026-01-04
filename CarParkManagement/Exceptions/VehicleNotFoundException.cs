namespace CarParkManagement.Exceptions;

public class VehicleNotFoundException(string vehicleReg) : Exception($"Vehicle with registration {vehicleReg} was not found")
{
}
