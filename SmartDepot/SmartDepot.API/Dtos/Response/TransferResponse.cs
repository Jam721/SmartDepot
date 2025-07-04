﻿using System.ComponentModel.DataAnnotations;

namespace SmartDepot.API.Dtos.Response;

/// <summary>
/// Представляет собой перемещение предмета с одного склада на другой.
/// </summary>
public class TransferResponse
{
    public int Id { get; set; }

    /// <summary>Ссылка на перемещаемый предмет</summary>
    [Required]
    [Display(Name ="Номер предмета")]
    public int ItemId { get; set; }

    /// <summary>Склад, откуда был перемещён предмет</summary>
    [Required]
    [Display(Name ="Склад из")]
    public int FromWarehouseId { get; set; }

    /// <summary>Склад, куда был перемещён предмет</summary>
    [Required]
    [Display(Name ="Склад в")]
    public int ToWarehouseId { get; set; }

    /// <summary>Дата и время перемещения</summary>
    [Required]
    [Display(Name ="Дата")]
    public DateTime TransferredAt { get; set; }
}