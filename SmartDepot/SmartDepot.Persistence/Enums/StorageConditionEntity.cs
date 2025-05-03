namespace SmartDepot.Persistence.Enums;


/// <summary>
/// Флаговый перечень условий хранения для склада.
/// Позволяет комбинировать условия через битовые операции (например, Cold | Dry).
/// </summary>
[Flags]
public enum StorageConditionEntity
{
    /// <summary>Условия не указаны</summary>
    None = 0,

    /// <summary>Требует хранения при низких температурах</summary>
    /// <example>Холодильные камеры</example>
    Cold = 1,

    /// <summary>Требует отсутствия влажности</summary>
    /// <example>Склады с осушителями воздуха</example>
    Dry = 2,

    /// <summary>Требует защиты от света</summary>
    /// <example>Темные помещения без окон</example>
    Dark = 4,

    /// <summary>Требует постоянной циркуляции воздуха</summary>
    /// <example>Склады с вентиляционными системами</example>
    Ventilated = 8,

    /// <summary>Хрупкие предметы - требуется осторожное обращение</summary>
    /// <example>Стекло, керамика</example>
    Fragile = 16,

    /// <summary>Требует вертикального хранения</summary>
    /// <example>Картины, листовые материалы</example>
    Upright = 32
}