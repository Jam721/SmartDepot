using System.ComponentModel.DataAnnotations;

namespace SmartDepot.API.Dtos.Request;

/// <summary>
/// Представляет бронирование предмета на определённый период.
/// </summary>
public class ReservationRequest
{
    /// <summary>Ссылка на забронированный предмет</summary>
    [Required]
    [Display(Name = "Номер предмета")]
    public int ItemId { get; set; }

    /// <summary>Дата начала бронирования</summary>
    [Required]
    [Display(Name = "Начало брони")]
    public DateTime ReservedFrom { get; set; }

    /// <summary>Дата окончания бронирования</summary>
    [Required]
    [Display(Name = "Конец брони")]
    public DateTime ReservedUntil { get; set; }
}