using CarParkManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Data;

public class ParkingSpaceDbContext(DbContextOptions<ParkingSpaceDbContext> options) : DbContext(options)
{
    public DbSet<ParkingSpace> ParkedVehicles => Set<ParkingSpace>();
}
