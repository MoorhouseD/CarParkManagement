namespace CarParkManagement.Models;

public class CarParkOptions
{
    public const string CarPark = "CarPark";

    public int TotalSpaces { get; set; } = 0;
    public Charge[]? ParkingCharges { get; set; } = null;
    public Charge? StandingCharge { get; set; } = null;
}
