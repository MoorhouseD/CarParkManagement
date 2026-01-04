using CarParkManagement.Enums;
using CarParkManagement.Exceptions;
using CarParkManagement.Models;

namespace CarParkManagement.Services;

public class ParkingService : IParkingService
{
    public int TotalSpaces { get; private set; }

    readonly ParkingSpace?[] _parkingSpaces;
    public int OccupiedSpacesCount => _parkingSpaces.Count(x => x != null);

    readonly IParkingChargeService _parkingChargeService;

    public ParkingService(IParkingChargeService parkingChargeService, IConfiguration configuration)
    {
        _parkingChargeService = parkingChargeService;
        CarParkOptions? options = configuration.GetSection(CarParkOptions.CarPark).Get<CarParkOptions>();

        if (options == null)
        {
            throw new NullReferenceException("Car Park configuration not available.");
        }

        TotalSpaces = options.TotalSpaces;

        _parkingSpaces = new ParkingSpace[TotalSpaces];
    }

    public int CountAvailableSpaces()
    {
        return TotalSpaces - OccupiedSpacesCount;
    }

    public async Task<ParkVehicleResponse> ParkVehicle(string vehicleReg, VehicleType vehicleType)
    {
        // Create vehicle
        var vehicle = new Vehicle(vehicleReg, vehicleType);

        //Optional store vehicle for future use

        //Get Next Space
        if (!TryGetNextAvailableSpace(out var parkingSpaceIndex))
        {
            throw new CarParkFullException();
        }

        // Park vehicle
        var parkingSpace = new ParkingSpace
        {
            ParkedVehicle = vehicle,
            ParkingTime = DateTime.UtcNow,
            SpaceNumber = parkingSpaceIndex + 1
        };

        _parkingSpaces[parkingSpaceIndex] = parkingSpace;

        return new ParkVehicleResponse(vehicleReg, parkingSpace.SpaceNumber, parkingSpace.ParkingTime.Value);
    }

    public async Task<VehicleExitResponse> ExitCarPark(string vehicleReg)
    {
        var parkingSpaceDetails = _parkingSpaces.SingleOrDefault(space => space != null && space.ParkedVehicle?.VehicleReg == vehicleReg);

        if (parkingSpaceDetails == null)
        {
            throw new VehicleNotFoundException(vehicleReg);
        }

        //Determine fee
        var vehicle = parkingSpaceDetails.ParkedVehicle;
        var entryTime = parkingSpaceDetails.ParkingTime!.Value;
        var exitTime = DateTime.UtcNow;

        var fee = _parkingChargeService.CalculateCharge(vehicle!.VehicleType, entryTime, exitTime);

        //De-allocate space
        _parkingSpaces[parkingSpaceDetails.SpaceNumber - 1] = null;

        return new VehicleExitResponse(vehicleReg, fee, entryTime, exitTime);
    }

    public async Task<CarParkSummaryResponse> GetCarParkState()
    {
        return await Task.FromResult(new CarParkSummaryResponse(CountAvailableSpaces(), OccupiedSpacesCount));
    }

    bool TryGetNextAvailableSpace(out int parkingSpaceIndex)
    {   
        if(OccupiedSpacesCount == TotalSpaces)
        {
            parkingSpaceIndex = -1;
            return false;
        }

        parkingSpaceIndex = Array.IndexOf(_parkingSpaces, null);
        return true;
    }
}
