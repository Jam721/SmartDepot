using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartDepot.Persistence.Entities.Enums;

namespace SmartDepot.Persistence.Entities;


/// <summary>
/// Это штука, которую мы храним на складе. Может быть ящик, мешок, что угодно.
/// </summary>
public class ItemEntity
{
    /// <summary>Уникальный идентификатор предмета</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>Название предмета (например, "Ящик с чаем")</summary>
    public string Name { get; set; }

    /// <summary>Вес предмета в килограммах</summary>
    public double WeightKg { get; set; }

    /// <summary>Объём предмета в кубометрах</summary>
    public double VolumeCubicMeters { get; set; }

    /// <summary>Фраза или характер предмета (например, "Я хрупкий!")</summary>
    public string? Quote { get; set; }

    /// <summary>Условия хранения предмета</summary>
    public StorageConditionEntity StorageConditions { get; set; }

    /// <summary>Склад, где хранится предмет</summary>
    public int WarehouseEntityId { get; set; }
    public WarehouseEntity? Warehouse { get; set; }
}
