using CarParkManagement.Enums;
using CarParkManagement.Exceptions;
using CarParkManagement.Models;

namespace CarParkManagement.Services;

public class ParkingChargeService : IParkingChargeService
{
    readonly Dictionary<VehicleType, Charge> _vehicleCharges = [];
    readonly Charge _standingCharge;

    public ParkingChargeService(IConfiguration configuration)
    {
        CarParkOptions? options = configuration.GetSection(CarParkOptions.CarPark).Get<CarParkOptions>();

        if (options == null)
        {
            throw new NullReferenceException("Car Park configuration not available.");
        }

        if (options.ParkingCharges == null)
        {
            throw new NullReferenceException("Car Park charges configuration not available.");
        }

        if (options.StandingCharge == null)
        {
            throw new NullReferenceException("Car Park standing charge configuration not available.");
        }

        _vehicleCharges = options.ParkingCharges
                                 .Select(x => new KeyValuePair<VehicleType, Charge>(Enum.Parse<VehicleType>(x.Description), x))
                                 .ToDictionary(k => k.Key, v => v.Value);
        _standingCharge = options.StandingCharge;
    }

    public decimal CalculateCharge(VehicleType vehicleType, DateTime startTime, DateTime exitTime)
    {
        if(!_vehicleCharges.TryGetValue(vehicleType, out var charge))
        {
            throw new ParkingChargeException($"The charge for a vehicle type of {vehicleType} has not been set.");
        }

        if (charge == null)
        {
            throw new InvalidOperationException("An error occurred calculating the parking fee. Please see a parking attendant.");
        }
        
        var stayInMinutes = (int)Math.Ceiling(exitTime.Subtract(startTime).TotalMinutes);

        if (stayInMinutes <= 0)
        {
            return 0m;
        }

        // compute per-minute rate and charge per minute parked
        var perMinuteRate = charge.Rate / charge.TimeframeInMinutes;
        var stayTimeFee = perMinuteRate * stayInMinutes;

        // standing charge applies every complete standing timeframe (e.g., every 5 minutes)
        var standingTimeframes = stayInMinutes / _standingCharge.TimeframeInMinutes; // integer division => floor
        var standingCharge = standingTimeframes * _standingCharge.Rate;

        return stayTimeFee + standingCharge;
    }

    //TODO: Update later with Charge type
    //public void SetChargePerMinute(VehicleType vehicleType, decimal chargePerMinute)
    //{
    //    if (chargePerMinute < 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(chargePerMinute), "Charge cannot be less than zero.");
    //    }

    //    if (!_vehicleCharges.TryAdd(vehicleType, chargePerMinute))
    //    {
    //        _vehicleCharges[vehicleType] = chargePerMinute;
    //    }
    //}

    //TODO: Update later with Charge type
    //public void SetStandingCharge(decimal standingCharge)
    //{
    //    if (standingCharge < 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(standingCharge), "Charge cannot be less than zero.");
    //    }

    //    StandingCharge = standingCharge;
    //}
}
