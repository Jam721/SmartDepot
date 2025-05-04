using Microsoft.AspNetCore.Mvc;
using SmartDepot.Application.Interfaces.Services;

namespace SmartDepot.API.Controllers;

/// <summary>
/// Контроллер советов и статуса от Петровича — эксперта по складам.
/// </summary>
[ApiController]
[Route("api/petrovich/")]
public class PetrovichController : ControllerBase
{
    private readonly IPertrovichService _service;

    /// <summary>
    /// Конструктор контроллера Петровича.
    /// </summary>
    /// <param name="service">Сервис с бизнес-логикой Петровича.</param>
    public PetrovichController(IPertrovichService service)
    {
        _service = service;
    }

    /// <summary>
    /// Получить житейский совет от Петровича.
    /// </summary>
    /// <returns>Текст совета.</returns>
    /// <response code="200">Успешно получен совет</response>
    [HttpGet("advice")]
    public async Task<IActionResult> GetPetrovichAdvice()
    {
        var advice = await _service.GetAdviceAsync();
        return Ok(advice);
    }

    /// <summary>
    /// Получить анализ состояния всех складов от Петровича
    /// </summary>
    /// <remarks>
    /// Подробный экспресс-аудит системы хранения с проверкой:
    /// <para>• Соответствия нагрузки допустимым параметрам складов</para>
    /// <para>• Соблюдения условий хранения для каждого предмета</para>
    /// <para>• Наличия подозрительных логистических операций</para>
    /// <para>• Устаревших и проблемных позиций</para>
    /// 
    /// Отчёт формируется в виде структурированного текста с выделением критичных замечаний.
    /// </remarks>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции</param>
    /// <returns>Текст отчёта в формате plain text</returns>
    /// <response code="200">Отчёт успешно сформирован</response>
    [HttpGet("status")]
    public async Task<IActionResult> GetPetrovichStatus(CancellationToken cancellationToken)
    {
        var status = await _service.GetWarehouseStatusAsync(cancellationToken);
        return Ok(status);
    }
    
    
    /// <summary>
    /// Добавить тестовые данные в систему
    /// </summary>
    /// <remarks>
    /// Создаёт 5 тестовых складов и 20 предметов с соблюдением всех условий хранения.
    /// Все объекты создаются с валидными параметрами и правильным распределением по складам.
    /// </remarks>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции</param>
    /// <returns>Сообщение о результате операции</returns>
    /// <response code="200">Тестовые данные успешно добавлены</response>
    [HttpPost("TestData")]
    public async Task<IActionResult> PostPetrovichTestData(CancellationToken cancellationToken)
    {
        var mes = await _service.TestDataAsync(cancellationToken);
        return Ok(mes);
    }
}
