using System.ComponentModel.DataAnnotations;
using SmartDepot.API.Enums;

namespace SmartDepot.API.Dtos.Request;

/// <summary>
/// Это штука, которую мы храним на складе. Может быть ящик, мешок, что угодно.
/// </summary>
public class ItemRequest
{
    /// <summary>Название предмета (например, "Ящик с чаем")</summary>
    [Required]
    [Display(Name = "Название предмета")]
    public string Name { get; set; }

    /// <summary>Вес предмета в килограммах</summary>
    [Required]
    [Display(Name = "Вес предмета(кг)")]
    [Range(1, 2000)]
    public double WeightKg { get; set; }

    /// <summary>Объём предмета в кубометрах</summary>
    [Required]
    [Display(Name = "Объем предмета(м^3)")]
    [Range(1, 100)]
    public double VolumeCubicMeters { get; set; }

    /// <summary>Фраза или характер предмета (например, "Я хрупкий!")</summary>
    [Display(Name = "Фраза предмета")]
    public string Quote { get; set; }

    /// <summary>Условия хранения предмета. Выбирай несколько через ctrl</summary>
    [Required]
    [Display(Name = "Условия хранения")]
    public List<StorageConditionRequest> StorageConditions { get; set; }

    /// <summary>Склад, где хранится предмет</summary>
    [Required]
    [Display(Name = "Номер склада")]
    public int WarehouseId { get; set; }
}