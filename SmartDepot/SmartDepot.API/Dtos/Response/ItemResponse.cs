namespace SmartDepot.API.Dtos.Response;

/// <summary>
/// Это штука, которую мы храним на складе. Может быть ящик, мешок, что угодно.
/// </summary>
public class ItemResponse
{
    public int Id { get; set; }
    
    /// <summary>Название предмета (например, "Ящик с чаем")</summary>
    public string Name { get; set; }

    /// <summary>Вес предмета в килограммах</summary>
    public double WeightKg { get; set; }

    /// <summary>Объём предмета в кубометрах</summary>
    public double VolumeCubicMeters { get; set; }

    /// <summary>Фраза или характер предмета (например, "Я хрупкий!")</summary>
    public string Quote { get; set; }

    /// <summary>Условия хранения предмета</summary>
    public List<string> StorageConditions { get; set; }

    /// <summary>Склад, где хранится предмет</summary>
    public int WarehouseId { get; set; }
}