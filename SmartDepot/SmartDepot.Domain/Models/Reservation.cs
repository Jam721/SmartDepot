namespace SmartDepot.Domain.Models;

/// <summary>
/// Представляет бронирование предмета на определённый период.
/// </summary>
public class Reservation
{
    /// <summary>Уникальный идентификатор бронирования</summary>
    public int Id { get; set; }

    /// <summary>Ссылка на забронированный предмет</summary>
    public int ItemId { get; set; }
    public Item? Item { get; set; }

    /// <summary>Дата начала бронирования</summary>
    public DateTime ReservedFrom { get; set; }

    /// <summary>Дата окончания бронирования</summary>
    public DateTime ReservedUntil { get; set; }
}
