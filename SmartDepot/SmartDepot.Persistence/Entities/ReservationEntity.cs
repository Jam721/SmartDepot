using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDepot.Persistence.Entities;


/// <summary>
/// Представляет бронирование предмета на определённый период.
/// </summary>
public class ReservationEntity
{
    /// <summary>Уникальный идентификатор бронирования</summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>Ссылка на забронированный предмет</summary>
    public int ItemEntityId { get; set; }
    public ItemEntity? Item { get; set; }

    /// <summary>Дата начала бронирования</summary>
    public DateTime ReservedFrom { get; set; }

    /// <summary>Дата окончания бронирования</summary>
    public DateTime ReservedUntil { get; set; }
}
