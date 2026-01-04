namespace CarParkManagement.Models;

public class Charge
{
    public required int Id { get; set; }
    public required string Description { get; set; }
    public required decimal Rate { get; set; }
    public required int TimeframeInMinutes { get; set; }
}
