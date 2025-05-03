using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDepot.Persistence.Entities;


/// <summary>
/// Представляет собой перемещение предмета с одного склада на другой.
/// </summary>
public class TransferEntity
{
    /// <summary>Уникальный идентификатор перемещения</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>Ссылка на перемещаемый предмет</summary>
    public int ItemEntityId { get; set; }
    public ItemEntity? Item { get; set; }

    /// <summary>Склад, откуда был перемещён предмет</summary>
    public int FromWarehouseId { get; set; }
    public WarehouseEntity? FromWarehouse { get; set; }

    /// <summary>Склад, куда был перемещён предмет</summary>
    public int ToWarehouseId { get; set; }
    public WarehouseEntity? ToWarehouse { get; set; }

    /// <summary>Дата и время перемещения</summary>
    public DateTime TransferredAt { get; set; }
}
