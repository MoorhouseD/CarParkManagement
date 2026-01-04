namespace CarParkManagement.Exceptions;

public class CarParkFullException : Exception
{
    public CarParkFullException() : base("All spaces in the car park are occupied.") { }
}
