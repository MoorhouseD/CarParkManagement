using CarParkManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CarParkManagement.Data;

public class ParkingSpaceDbContext(DbContextOptions<ParkingSpaceDbContext> options) : DbContext(options)
{
    public DbSet<ParkingSpace> ParkedVehicles => Set<ParkingSpace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSpace>().HasKey(p => p.SpaceNumber);
        modelBuilder.Entity<Vehicle>().HasKey(v => v.VehicleReg);
        base.OnModelCreating(modelBuilder);
    }
}
