using SmartDepot.Domain.Enums;

namespace SmartDepot.Infrastructure;

public static class GetFlagsFromInt
{
    public static List<StorageCondition> GetFlags(int value)
    {
        var conditionFlags = (StorageCondition)value;
        return Enum.GetValues<StorageCondition>()
            .Where(flag => flag != StorageCondition.None && conditionFlags.HasFlag(flag))
            .ToList();
    }
}