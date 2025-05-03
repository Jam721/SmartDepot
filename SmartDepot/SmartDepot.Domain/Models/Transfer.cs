namespace SmartDepot.Domain.Models;

/// <summary>
/// Представляет собой перемещение предмета с одного склада на другой.
/// </summary>
public class Transfer
{
    /// <summary>Уникальный идентификатор перемещения</summary>
    public int Id { get; set; }

    /// <summary>Ссылка на перемещаемый предмет</summary>
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    /// <summary>Склад, откуда был перемещён предмет</summary>
    public int FromWarehouseId { get; set; }
    public Warehouse? FromWarehouse { get; set; }

    /// <summary>Склад, куда был перемещён предмет</summary>
    public int ToWarehouseId { get; set; }
    public Warehouse? ToWarehouse { get; set; }

    /// <summary>Дата и время перемещения</summary>
    public DateTime TransferredAt { get; set; }
}
