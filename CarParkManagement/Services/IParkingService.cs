using CarParkManagement.Enums;
using CarParkManagement.Models;
using CarParkManagement.Exceptions;

namespace CarParkManagement.Services;

public interface IParkingService
{
    int TotalSpaces { get; }
    int OccupiedSpacesCount { get; }

    int CountAvailableSpaces();

    /// <summary>
    /// Parks a registered vehicle in the next available space, also sets the entry time.
    /// </summary>
    /// <param name="vehicleReg">The registration of the vehicle</param>
    /// <param name="vehicleType">An enum referring to the vehicle size</param>
    /// <returns>Returns reference information about the vehicle, the allocated space, and the entry time.</returns>
    /// <exception cref="CarParkFullException"></exception>
    Task<ParkVehicleResponse> ParkVehicle(string vehicleReg, VehicleType vehicleType);

    /// <summary>
    /// Calculates the parking fee for a vehicle and de-allocates the parking space
    /// </summary>
    /// <param name="vehicleReg">The registration of the vehicle</param>
    /// <returns>A response indicating the parking fee, entry time and exit time of the vehicle</returns>
    /// <exception cref="VehicleNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task<VehicleExitResponse> ExitCarPark(string vehicleReg);

    /// <summary>
    /// Queries the state of the car park
    /// </summary>
    /// <returns>Returns a count of available and occupied spaces</returns>
    Task<CarParkSummaryResponse> GetCarParkState();
}
