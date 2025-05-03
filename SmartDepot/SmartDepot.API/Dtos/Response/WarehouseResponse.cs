namespace SmartDepot.API.Dtos.Response;

public class WarehouseResponse
{
    public int Id { get; set; }
    
    /// <summary>Название склада (например, "Северный-1")</summary>
    public string Name { get; set; }

    /// <summary>Максимальный вес, который склад может выдержать (в кг)</summary>
    public double MaxWeightCapacity { get; set; }

    /// <summary>Максимальный объём, который склад может вмещать (в м³)</summary>
    public double MaxVolumeCapacity { get; set; }

    /// <summary>Условия, которые поддерживает склад (например, "Холод", "Сухость")</summary>
    public List<string> SupportedConditions { get; set; }
}