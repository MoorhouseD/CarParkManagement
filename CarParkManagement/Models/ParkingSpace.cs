namespace CarParkManagement.Models;

public class ParkingSpace
{
    public int SpaceNumber { get; set; }

    public Vehicle? ParkedVehicle { get; set; }
    public DateTime? ParkingTime { get; set; }

    public bool IsOccupied => ParkedVehicle != null;

    //Other useful properties could be:
    //enum State (Occupied/Available) instead of bool
    //enum Restriction (Disabled, Bikes Only, Electric Charging Only, etc.)
}
