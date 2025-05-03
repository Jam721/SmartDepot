using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartDepot.Persistence.Enums;

namespace SmartDepot.Persistence.Entities;


/// <summary>
/// Склад — место хранения предметов. Может быть холодным, тёплым, огромным или крошечным.
/// </summary>
public class WarehouseEntity
{
    /// <summary>Уникальный идентификатор склада</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>Название склада (например, "Северный-1")</summary>
    public string Name { get; set; }

    /// <summary>Максимальный вес, который склад может выдержать (в кг)</summary>
    public double MaxWeightCapacity { get; set; }

    /// <summary>Максимальный объём, который склад может вмещать (в м³)</summary>
    public double MaxVolumeCapacity { get; set; }

    /// <summary>Условия, которые поддерживает склад (например, "Холод", "Сухость")</summary>
    public StorageConditionEntity SupportedConditions { get; set; }
}
