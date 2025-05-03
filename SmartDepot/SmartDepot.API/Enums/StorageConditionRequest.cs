using System.ComponentModel.DataAnnotations;

namespace SmartDepot.API.Enums;

/// <summary>
/// Флаговый перечень условий хранения для склада.
/// Позволяет комбинировать условия через битовые операции (например, Cold | Dry).
/// </summary>
[Flags]
public enum StorageConditionRequest
{
    /// <summary>Условия не указаны</summary>
    [Display(Name = "Не указаны")]
    None = 0,

    /// <summary>Требует хранения при низких температурах</summary>
    [Display(Name = "Холод")]
    Cold = 1,

    /// <summary>Требует отсутствия влажности</summary>
    [Display(Name = "Сухость")]
    Dry = 2,

    /// <summary>Требует защиты от света</summary>
    [Display(Name = "Темнота")]
    Dark = 4,

    /// <summary>Требует постоянной циркуляции воздуха</summary>
    [Display(Name = "Вентиляция")]
    Ventilated = 8,

    /// <summary>Хрупкие предметы - требуется осторожное обращение</summary>
    [Display(Name = "Хрупкие")]
    Fragile = 16,

    /// <summary>Требует вертикального хранения</summary>
    [Display(Name = "Вертикальное хранение")]
    Upright = 32
}
