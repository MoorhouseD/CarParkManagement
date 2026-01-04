using System.ComponentModel;

namespace CarParkManagement.Enums;

public enum VehicleType
{
    [Description("Small Car")]
    SmallCar = 1,
    [Description("Medium Car")]
    MediumCar = 2,
    [Description("Large Car")]
    LargeCar = 3
}
