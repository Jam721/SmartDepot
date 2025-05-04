using System.Text;
using SmartDepot.Application.Data;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Application.Interfaces.Services;
using SmartDepot.Domain.Enums;
using SmartDepot.Domain.Models;

namespace SmartDepot.Application.Services;

public class PetrovichService : IPertrovichService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IItemRepository _itemRepository;
    private readonly ITransferRepository _transferRepository;


    public PetrovichService(
        IWarehouseRepository warehouseRepository,
        IItemRepository itemRepository,
        ITransferRepository transferRepository)
    {
        _warehouseRepository = warehouseRepository;
        _itemRepository = itemRepository;
        _transferRepository = transferRepository;
    }

    public Task<string> GetAdviceAsync()
    {
        var random = new Random();

        var sovety = SovetyOtPetrovichaLol.GetSovety();

        var advice = random.Next(0, sovety.Count - 1);

        return Task.FromResult(sovety[advice]);
    }


    public async Task<string> GetWarehouseStatusAsync(CancellationToken cancellationToken)
    {
        var warehouses = await _warehouseRepository.GetWarehousesAsync(cancellationToken);
        var items = await _itemRepository.GetAllItemsAsync(cancellationToken);

        var reportBuilder = new StringBuilder();
        reportBuilder.AppendLine("🧔 Петрович осмотрел склады и говорит:");
        
        
        if (warehouses is null) reportBuilder.AppendLine("Дебилойд нафиг ты хочешь проанализировать склады их же нет!");
        else
            foreach (var warehouse in warehouses)
            {
                if (items is null) reportBuilder.AppendLine("Добавь предметы, дебил!");
                else
                {
                    var warehouseItems = items.Where(i => i.WarehouseId == warehouse.Id).ToList();
                    var totalWeight = warehouseItems.Sum(i => i.WeightKg);
                    var totalVolume = warehouseItems.Sum(i => i.VolumeCubicMeters);
                    var warehouseMessages = new List<string>();

                    if (totalWeight > warehouse.MaxWeightCapacity)
                    {
                        warehouseMessages.Add("Весит больше, чем холодильник с чугуном — пора разгрузить!");
                    }

                    if (totalVolume > warehouse.MaxVolumeCapacity)
                    {
                        warehouseMessages.Add("По объёму как тёщин погреб — не влезет уже ничего.");
                    }

                    foreach (var item in warehouseItems)
                    {
                        if ((item.StorageConditions & warehouse.SupportedConditions) != item.StorageConditions)
                        {
                            warehouseMessages.Add($"\"{item.Name}\" тут как рыба на сковородке — не те условия!");
                        }
                    }

                    var fragileCount = warehouseItems.Count(i => (i.StorageConditions & StorageCondition.Fragile) != 0);
                    if (fragileCount > 10)
                    {
                        warehouseMessages.Add("Столько хрупкого, что даже мой сон надёжнее. Распихай это по складам.");
                    }

                    if (warehouseItems.Count < 2)
                    {
                        warehouseMessages.Add("Пустой как мой холодильник перед зарплатой. Закинь чего-нибудь.");
                    }

                    if (!warehouseMessages.Any())
                    {
                        warehouseMessages.Add("Всё тихо и складно. Даже мыши тут уважают порядок!");
                    }

                    reportBuilder.AppendLine($"\n🏠 Склад \"{warehouse.Name}\":");
                    foreach (var msg in warehouseMessages)
                    {
                        reportBuilder.AppendLine($" - {msg}");
                    }
                }
            }
        
        var transfers = await _transferRepository.GetAllTransfersAsync(cancellationToken);
        
        if (transfers is not null && warehouses is not null)
        {
            var hourAgo = DateTime.UtcNow.AddHours(-1);
            var recentTransfers = transfers!.Where(t => t.TransferredAt >= hourAgo).ToList();
            if (recentTransfers.Count > 10)
            {
                reportBuilder.AppendLine(
                    $"\n🚛 Перемещений было аж {recentTransfers.Count} за последний час — ты чего, марафон по логистике устроил?");
            }

            var forgottenItems = items.Where(i =>
                transfers.Any(t => t.ItemId == i.Id && t.TransferredAt > DateTime.UtcNow.AddDays(-30))
            ).ToList();
            if (forgottenItems.Count > 5)
            {
                reportBuilder.AppendLine(
                    $"\n🕸️ Пылятся {forgottenItems.Count} предметов. Может, это экспонаты? Или просто забыли про них.");
            }
        }
        else reportBuilder.AppendLine("БРО У ТЕБЯ ТОВАРЫ НЕ ПЕРЕМЕЩАЮТСЯ! Ну а так пофиг");

        return reportBuilder.ToString();
    }

    public async Task<string> TestDataAsync(CancellationToken cancellationToken)
    {
        var warehouses = new List<Warehouse>
        {
            new Warehouse
            {
                Name = "❄️ Ледяной Бункер Пингвинов",
                MaxWeightCapacity = 15_000,
                MaxVolumeCapacity = 800,
                SupportedConditions = StorageCondition.Ventilated | StorageCondition.Upright 
                                      | StorageCondition.Fragile | StorageCondition.Dark 
                                      | StorageCondition.Cold,
            },
            new Warehouse
            {
                Name = "🔥 Склад-Сауна 'Как в Аду'",
                MaxWeightCapacity = 25_000,
                MaxVolumeCapacity = 1200,
                SupportedConditions = StorageCondition.Upright | StorageCondition.Dry 
                                      | StorageCondition.Ventilated,
            },
            new Warehouse
            {
                Name = "🎪 Цирк-склад 'Трюки и Коробки'",
                MaxWeightCapacity = 18_000,
                MaxVolumeCapacity = 950,
                SupportedConditions = StorageCondition.Dry | StorageCondition.Fragile 
                                      | StorageCondition.Upright,
            },
            new Warehouse
            {
                Name = "💀 Хранилище Запретного Артефакта",
                MaxWeightCapacity = 30_000,
                MaxVolumeCapacity = 2000,
                SupportedConditions = StorageCondition.Cold | StorageCondition.Fragile 
                                      | StorageCondition.Dark,
            },
            new Warehouse
            {
                Name = "🚀 Склад-Ракета 'На Марс!'",
                MaxWeightCapacity = 50_000,
                MaxVolumeCapacity = 3000,
                SupportedConditions = StorageCondition.Dry | StorageCondition.Upright,
            }
        };

        var createdWarehouses = new List<Warehouse>();
        foreach (var warehouse in warehouses)
        {
            var created = await _warehouseRepository.CreateWarehouseAsync(warehouse, cancellationToken);
            createdWarehouses.Add(created);
        }

        var items = new List<Item>
        {
            
            new Item
            {
                Name = "🧊 Мороженое 'Слеза Снеговика'",
                WeightKg = 150,
                VolumeCubicMeters = 2.5,
                StorageConditions = StorageCondition.Cold,
                WarehouseId = createdWarehouses[0].Id,
                Quote = "Если растаю — превращусь в лужицу грусти."
            },
            new Item
            {
                Name = "🐧 Партия говорящих пингвинов",
                WeightKg = 300,
                VolumeCubicMeters = 4.0,
                StorageConditions = StorageCondition.Cold | StorageCondition.Upright,
                WarehouseId = createdWarehouses[0].Id,
                Quote = "Требуем рыбки и Netflix с документалками про Антарктиду!"
            },

            // Для "🔥 Склад-Сауна 'Как в Аду'"
            new Item
            {
                Name = "🌶️ Огненный Соус 'Дыхание Дракона'",
                WeightKg = 800,
                VolumeCubicMeters = 8.5,
                StorageConditions = StorageCondition.Dry,
                WarehouseId = createdWarehouses[1].Id,
                Quote = "Хранить вдали от туалетной бумаги и слабых духом."
            },
            new Item
            {
                Name = "🧨 Новогодние фейерверки (просроченные)",
                WeightKg = 450,
                VolumeCubicMeters = 5.2,
                StorageConditions = StorageCondition.Ventilated,
                WarehouseId = createdWarehouses[1].Id,
                Quote = "Могут взорваться от чихания. Не рекомендуем дарить тёще."
            },

            // Для "🎪 Цирк-склад 'Трюки и Коробки'"
            new Item
            {
                Name = "🎪 Шарики для жонглирования (с сюрпризом)",
                WeightKg = 1200,
                VolumeCubicMeters = 15.0,
                StorageConditions = StorageCondition.Fragile,
                WarehouseId = createdWarehouses[2].Id,
                Quote = "Внутри не воздух, а конфетти. И одна пчела. Может быть."
            },
            new Item
            {
                Name = "🤡 Костюм клоуна-невидимки",
                WeightKg = 750,
                VolumeCubicMeters = 9.3,
                StorageConditions = StorageCondition.Dry,
                WarehouseId = createdWarehouses[2].Id,
                Quote = "Пустой костюм. Или нет? *зловещий смех*"
            },

            // Для "💀 Хранилище Запретного Артефакта"
            new Item
            {
                Name = "📜 Свиток 'Как разозлить Петровича'",
                WeightKg = 2500,
                VolumeCubicMeters = 18.0,
                StorageConditions = StorageCondition.Dark | StorageCondition.Fragile,
                WarehouseId = createdWarehouses[3].Id,
                Quote = "Прочтение вслух = вечная блокировка в API."
            },
            new Item
            {
                Name = "🕯️ Свеча, которая светит только когда никто не смотрит",
                WeightKg = 1800,
                VolumeCubicMeters = 12.5,
                StorageConditions = StorageCondition.Dark,
                WarehouseId = createdWarehouses[3].Id,
                Quote = "Проверено: работает. И это пугает."
            },

            // Для "🚀 Склад-Ракета 'На Марс!'"
            new Item
            {
                Name = "🧑🚀 Костюм космонавта (размер XXXL)",
                WeightKg = 15000,
                VolumeCubicMeters = 45.0,
                WarehouseId = createdWarehouses[4].Id,
                Quote = "Для тех, кто верит, что марсиане любят пончики."
            },
            new Item
            {
                Name = "👽 Инопланетный кактус в горшке",
                WeightKg = 8500,
                VolumeCubicMeters = 32.0,
                StorageConditions = StorageCondition.Dry,
                WarehouseId = createdWarehouses[4].Id,
                Quote = "Поливать кофе. Цветёт по четвергам. Кусается."
            }
        };

        foreach (var item in items)
        {
            await _itemRepository.CreateItemAsync(item, cancellationToken);
        }

        return "🎉 Добавлено: 5 прокомментированных складов, 10 безумных предметов. Петрович одобряет!";
    }
}