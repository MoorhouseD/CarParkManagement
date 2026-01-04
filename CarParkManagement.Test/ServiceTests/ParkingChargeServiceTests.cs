using CarParkManagement.Enums;
using CarParkManagement.Exceptions;
using CarParkManagement.Models;
using CarParkManagement.Services;
using Microsoft.Extensions.Configuration;

namespace CarParkManagement.Test.ServiceTests;

public class ParkingChargeServiceTests
{
    ParkingChargeService _chargeService;

    readonly DateTime ParkingTime = new(2026, 1, 1, 10, 30, 0);

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"CarPark:ParkingCharges:0:Id", "1"},
            {"CarPark:ParkingCharges:0:Description", "SmallCar"},
            {"CarPark:ParkingCharges:0:Rate", "0.10"},
            {"CarPark:ParkingCharges:0:TimeframeInMinutes", "1"},
            {"CarPark:ParkingCharges:1:Id", "2"},
            {"CarPark:ParkingCharges:1:Description", "MediumCar"},
            {"CarPark:ParkingCharges:1:Rate", "0.20"},
            {"CarPark:ParkingCharges:1:TimeframeInMinutes", "1"},
            {"CarPark:ParkingCharges:2:Id", "3"},
            {"CarPark:ParkingCharges:2:Description", "LargeCar"},
            {"CarPark:ParkingCharges:2:Rate", "0.40"},
            {"CarPark:ParkingCharges:2:TimeframeInMinutes", "1"},
            {"CarPark:StandingCharge:Id", "1"},
            {"CarPark:StandingCharge:Description", "StandingCharge"},
            {"CarPark:StandingCharge:Rate", "1.00"},
            {"CarPark:StandingCharge:TimeframeInMinutes", "5"}
        };

        var configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(inMemorySettings)
                                .Build();

        _chargeService = new ParkingChargeService(configuration);
    }

    [Test]
    public async Task CalculateCharge_SmallCar_ReturnsExpectedCharge()
    {
        var startTime = ParkingTime;
        var exitTime = ParkingTime.AddMinutes(10);
        var charge = _chargeService.CalculateCharge(VehicleType.SmallCar, startTime, exitTime);

        // For small car: per minute rate 0.10 * 10 = 1.0; standing charge every 5 minutes => 2.00 (floor 10/5 = 2) => total 3.0
        Assert.That(charge, Is.EqualTo(3.0m));
    }

    [Test]
    public async Task CalculateCharge_MediumCar_ReturnsExpectedCharge()
    {
        var startTime = ParkingTime;
        var exitTime = ParkingTime.AddMinutes(10);
        var charge = _chargeService.CalculateCharge(VehicleType.MediumCar, startTime, exitTime);

        // For medium car: per minute rate 0.20 * 10 = 2.0; standing charge every 5 minutes => 2.00 (floor 10/5 = 2) => total 4
        Assert.That(charge, Is.EqualTo(4.0m));
    }

    [Test]
    public async Task CalculateCharge_LargeCar_ReturnsExpectedCharge()
    {
        var startTime = ParkingTime;
        var exitTime = ParkingTime.AddMinutes(10);
        var charge = _chargeService.CalculateCharge(VehicleType.LargeCar, startTime, exitTime);

        // For large car: per minute rate 0.40 * 10 = 4.0; standing charge every 5 minutes => 2.00 (floor 10/5 = 2) => total 6
        Assert.That(charge, Is.EqualTo(6.0m));
    }

    [Test]
    public async Task CalculateCharge_UnknownVehicle_ThrowsException()
    {
        var startTime = ParkingTime;
        var exitTime = ParkingTime.AddMinutes(10);

        Assert.Throws<ParkingChargeException>(() => _chargeService.CalculateCharge((VehicleType)999, startTime, exitTime));
    }
}
