﻿using SmartDepot.Domain.Enums;

namespace SmartDepot.Domain.Models;

/// <summary>
/// Склад — место хранения предметов. Может быть холодным, тёплым, огромным или крошечным.
/// </summary>
public class Warehouse
{
    /// <summary>Уникальный идентификатор склада</summary>
    public int Id { get; set; }

    /// <summary>Название склада (например, "Северный-1")</summary>
    public string Name { get; set; }

    /// <summary>Максимальный вес, который склад может выдержать (в кг)</summary>
    public double MaxWeightCapacity { get; set; }

    /// <summary>Максимальный объём, который склад может вмещать (в м³)</summary>
    public double MaxVolumeCapacity { get; set; }

    /// <summary>Условия, которые поддерживает склад (например, "Холод", "Сухость")</summary>
    public StorageCondition SupportedConditions { get; set; }
}
