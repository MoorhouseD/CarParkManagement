using CarParkManagement.Enums;

namespace CarParkManagement.Services;

public interface IParkingChargeService
{
    decimal CalculateCharge(VehicleType vehicleType, DateTime startTime, DateTime exitTime);
    //void SetChargePerMinute(VehicleType vehicleType, decimal chargePerMinute);
    //void SetStandingCharge(decimal standingCharge);
}
