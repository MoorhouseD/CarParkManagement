using CarParkManagement.Enums;
using CarParkManagement.Exceptions;
using CarParkManagement.Services;
using FakeItEasy;
using Microsoft.Extensions.Configuration;

namespace CarParkManagement.Test.ServiceTests;

public class ParkingServiceTests
{
    IParkingChargeService _fakeParkingChargeService;
    IParkingService _parkingService;

    const int TotalSpaces = 10;

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"CarPark:TotalSpaces", $"{TotalSpaces}"},
        };

        var configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemorySettings)
                                .Build();

        _fakeParkingChargeService = A.Fake<IParkingChargeService>();

        _parkingService = new ParkingService(_fakeParkingChargeService, configuration);
    }

    [Test]
    public async Task ParkVehicle_AllocatesFirstAvailableSpace()
    {
        using (Assert.EnterMultipleScope())
        {
            var response = await _parkingService.ParkVehicle("ABC123", VehicleType.MediumCar);

            Assert.That(response.SpaceNumber, Is.EqualTo(1));
            Assert.That(_parkingService.OccupiedSpacesCount, Is.EqualTo(1));
            Assert.That(_parkingService.CountAvailableSpaces(), Is.EqualTo(TotalSpaces - 1));
        }
    }

    [Test]
    public async Task ExitVehicle_CalculateChargeAndClearsOccupiedSpace()
    {
        using (Assert.EnterMultipleScope())
        {
            var parkResponse = await _parkingService.ParkVehicle("XYZ999", VehicleType.SmallCar);

            //Simulate 5 minutes, small car charge (0.10 * 5) + standing charge (1.00) = 1.50
            A.CallTo(() => _fakeParkingChargeService.CalculateCharge(VehicleType.SmallCar, parkResponse.TimeIn, A<DateTime>.Ignored))
                .Returns(1.50m);

            var exitResponse = await _parkingService.ExitCarPark("XYZ999");

            Assert.That(exitResponse.VehicleCharge, Is.EqualTo(1.5m));
            Assert.That(_parkingService.OccupiedSpacesCount, Is.Zero);
            Assert.That(_parkingService.CountAvailableSpaces(), Is.EqualTo(TotalSpaces));
        }
    }

    [Test]
    public async Task ParkVehicle_MultipleVehiclesPark_OneExits_ShouldAssignSpacesCorrectly()
    {
        using (Assert.EnterMultipleScope())
        {
            //Park 3 vehicles
            var parkResponse1 = await _parkingService.ParkVehicle("ABC001", VehicleType.SmallCar);
            var parkResponse2 = await _parkingService.ParkVehicle("ABC002", VehicleType.MediumCar);
            var parkResponse3 = await _parkingService.ParkVehicle("ABC003", VehicleType.LargeCar);

            //Simulate 5 minutes, small car charge (0.20 * 5) + standing charge (1.00) = 2.00
            A.CallTo(() => _fakeParkingChargeService.CalculateCharge(VehicleType.MediumCar, parkResponse2.TimeIn, A<DateTime>.Ignored))
                .Returns(2.00m);

            var exitResponse = await _parkingService.ExitCarPark("ABC002");

            // Verify exit charge and space availability
            Assert.That(exitResponse.VehicleCharge, Is.EqualTo(2.00m));
            Assert.That(_parkingService.OccupiedSpacesCount, Is.EqualTo(2));
            Assert.That(_parkingService.CountAvailableSpaces(), Is.EqualTo(TotalSpaces - 2));

            // Now park another vehicle and ensure it takes the freed space (space number 2)
            var parkResponse4 = await _parkingService.ParkVehicle("ABC004", VehicleType.SmallCar);

            // Verify that the new vehicle gets space number 2
            Assert.That(parkResponse4.SpaceNumber, Is.EqualTo(2));
        }
    }

    [Test]
    public void ExitVehicle_NotFound_ThrowsVehicleNotFoundException()
    {
        Assert.ThrowsAsync<VehicleNotFoundException>(async () => await _parkingService.ExitCarPark("NOEXIST"));
    }
}
