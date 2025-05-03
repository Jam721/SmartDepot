using Microsoft.EntityFrameworkCore;
using SmartDepot.Persistence.Entities;

namespace SmartDepot.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<ReservationEntity> Reservations { get; set; }
    public DbSet<TransferEntity> Transfers { get; set; }
    public DbSet<WarehouseEntity> Warehouses { get; set; }
}