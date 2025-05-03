using System.ComponentModel.DataAnnotations;
using SmartDepot.API.Enums;

namespace SmartDepot.API.Dtos.Request;

public class WarehouseRequest
{
    /// <summary>Название склада (например, "Северный-1")</summary>
    [Required]
    [Display(Name = "Название склада")]
    public string Name { get; set; }

    /// <summary>Максимальный вес, который склад может выдержать (в кг)</summary>
    [Required]
    [Display(Name = "Макс. вес(кг)")]
    [Range(1, 10_000_000)]
    public double MaxWeightCapacity { get; set; }

    /// <summary>Максимальный объём, который склад может вмещать (в м³)</summary>
    [Required]
    [Display(Name = "Макс. объем(м^3)")]
    [Range(1, 10_000)]
    public double MaxVolumeCapacity { get; set; }

    /// <summary>Условия, которые поддерживает склад (например, "Холод", "Сухость"). Выбирай несколько через ctrl</summary>
    [Required]
    [Display(Name = "Условия хранения")]
    public List<StorageConditionRequest> SupportedConditions { get; set; }
}