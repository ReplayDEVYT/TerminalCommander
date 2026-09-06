using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Common.Models.Logging;
using SPTarkov.Server.Core.Utils.Cloners;
using SPTarkov.Server.Core.Services.Locales;
using SPTarkov.Server.Core.Models.Spt.Tables;
using HarmonyLib.Tools;

namespace TerminalCommander
{
    [Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
    public class AddCustomTraderHelper(
        ISptLogger<AddCustomTraderHelper> logger,
        ICloner cloner,
        TradersTable tradersTable,
        LocaleTable localeTable,
        LocaleService localeService)
    {

        public void SetTraderUpdateTime(TraderConfig traderConfig, TraderBase baseJson, int refreshTimeSecondsMin, int refreshTimeSecondsMax)
        {
            var traderRefreshRecord = new UpdateTime
            {
                TraderId = baseJson.Id,
                Seconds = new MinMax<int>(refreshTimeSecondsMin, refreshTimeSecondsMax)
            };

            traderConfig.UpdateTime.Add(traderRefreshRecord);
        }

        public void AddTraderWithEmptyAssortToDb(TraderBase traderDetailsToAdd)
        {
            var emptyTraderItemAssortObject = new TraderAssort
            {
                Items = [],
                BarterScheme = new Dictionary<MongoId, List<List<BarterScheme>>>(),
                LoyalLevelItems = new Dictionary<MongoId, int>()
            };

            var traderDataToAdd = new Trader
            {
                Assort = emptyTraderItemAssortObject,
                Base = cloner.Clone(traderDetailsToAdd),
                QuestAssort = new()
                {
                    { "Started", new() },
                    { "Success", new() },
                    { "Fail", new() }
                },
                Dialogue = []
            };

            if (!tradersTable.TryAdd(traderDetailsToAdd.Id, traderDataToAdd))
            {
            }
        }

        public void AddTraderToLocales(TraderBase baseJson, string firstName, string description)
        {
            // For each language, add locale for the new trader
            var locales = localeTable.Global;
            var newTraderId = baseJson.Id;
            var fullName = baseJson.Name;
            var nickName = baseJson.Nickname;
            var location = baseJson.Location;

            foreach (var (localeKey, localeKvP) in locales)
            {
                // We have to add a transformer here, because locales are lazy loaded due to them taking up huge space in memory
                // The transformer will make sure that each time the locales are requested, the ones added below are included
                localeKvP.AddTransformer(lazyloadedLocaleData =>
                {
                    lazyloadedLocaleData.Add($"{newTraderId} FullName", fullName);
                    lazyloadedLocaleData.Add($"{newTraderId} FirstName", firstName);
                    lazyloadedLocaleData.Add($"{newTraderId} Nickname", nickName);
                    lazyloadedLocaleData.Add($"{newTraderId} Location", location);
                    lazyloadedLocaleData.Add($"{newTraderId} Description", description);
                    return lazyloadedLocaleData;
                });
            }
        }

        public void OverwriteTraderAssort(string traderId, TraderAssort newAssorts)
        {
            if (!tradersTable.TryGetValue(traderId, out var traderToEdit))
            {
                logger.Warning($"Unable to update assorts for trader: {traderId}, they couldn't be found on the server");

                return;
            }

            traderToEdit.Assort = newAssorts;
        }
    }
}