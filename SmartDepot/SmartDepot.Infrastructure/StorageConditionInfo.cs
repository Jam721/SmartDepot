using SmartDepot.Domain.Enums;

namespace SmartDepot.Infrastructure;

public static class StorageConditionInfo
{
    private static readonly Dictionary<StorageCondition, string> Descriptions = new()
    {
        { StorageCondition.Cold, "Хранить в холодильнике (до 5°C)" },
        { StorageCondition.Dry, "Беречь от влаги" },
        { StorageCondition.Dark, "Избегать прямых солнечных лучей" },
        { StorageCondition.Ventilated, "Проветриваемое помещение" },
        { StorageCondition.Fragile, "Осторожно! Хрупкий предмет" },
        { StorageCondition.Upright, "Хранить в вертикальном положении" },
    };

    private static readonly Dictionary<StorageCondition, (double min, double max)> StorageConditionRanges = new()
    {
        { StorageCondition.Cold, (0, 5) },
        { StorageCondition.Dry, (5, 25) },
        { StorageCondition.Dark, (0, 30) }
    };

    public static string GetDescription(StorageCondition condition)
    {
        return Descriptions.GetValueOrDefault(condition, "Нет описания");
    }

    public static (double min, double max)? GetStorageConditionRange(StorageCondition condition)
    {
        return StorageConditionRanges.TryGetValue(condition, out var range) ? range : null;
    }
}