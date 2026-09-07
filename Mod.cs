using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Routers;
using SPTarkov.Server.Core.Utils;
using System.Reflection;
using TerminalCommander;
using Path = System.IO.Path;

namespace TerminalCommander;

// This record holds the various properties for your mod
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.mizmii.terminalcommander";
    public string Name { get; init; } = "TerminalCommander";
    public string Author { get; init; } = "mizmii";
    public List<string>? Contributors { get; init; } = [];
    public SemanticVersioning.Version Version { get; init; } = new("1.1.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public List<string>? Incompatibilities { get; init; } = [];
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; } = "https://github.com/sp-tarkov/server-mod-examples";
    public string License { get; init; } = "MIT";
    public bool HasPrepatcher { get; init; } = false;
}

[Injectable(TypePriority = OnLoadOrder.PostLoad + 1)]
public class AddTraderWithAssortJson(
    ModHelper modHelper,
    ImageRouter imageRouter,
    TraderConfig traderConfig,
    RagfairConfig ragfairConfig,
    TimeUtil timeUtil,
    AddCustomTraderHelper addCustomTraderHelper,
    WTTServerCommonLib.WTTServerCommonLib wttcommon
)
    : IOnLoad
{
    public async Task OnLoadAsync(CancellationToken cancellationToken)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());

        var RUAFImagePath = Path.Combine(pathToMod, "db/CustomTraders/RUAF/ruaf.jpg");
        var traderRUAF = modHelper.GetJsonDataFromFile<TraderBase>(pathToMod, "db/CustomTraders/RUAF/base.json");

        imageRouter.AddRoute(traderRUAF.Avatar.Replace(".jpg", ""), RUAFImagePath);
        addCustomTraderHelper.SetTraderUpdateTime(traderConfig, traderRUAF, timeUtil.GetHoursAsSeconds(1), timeUtil.GetHoursAsSeconds(2));
        addCustomTraderHelper.AddTraderWithEmptyAssortToDb(traderRUAF);
        addCustomTraderHelper.AddTraderToLocales(traderRUAF, "RUAF", "Bro!! Stop cheat!! How you see this!!");

        var CommandImagePath = Path.Combine(pathToMod, "db/CustomTraders/command/command.jpg");
        var traderCommand = modHelper.GetJsonDataFromFile<TraderBase>(pathToMod, "db/CustomTraders/command/base.json");

        imageRouter.AddRoute(traderCommand.Avatar.Replace(".jpg", ""), CommandImagePath);
        addCustomTraderHelper.SetTraderUpdateTime(traderConfig, traderCommand, timeUtil.GetHoursAsSeconds(1), timeUtil.GetHoursAsSeconds(2));
        addCustomTraderHelper.AddTraderWithEmptyAssortToDb(traderCommand);
        addCustomTraderHelper.AddTraderToLocales(traderCommand, "Yuri", "RUAF commander, in charge of the port. Open to trade with people he trusts.");

        var kermanImagePath = Path.Combine(pathToMod, "db/CustomTraders/kerman/kerman.jpg");
        var traderKerman = modHelper.GetJsonDataFromFile<TraderBase>(pathToMod, "db/CustomTraders/kerman/base.json");

        imageRouter.AddRoute(traderKerman.Avatar.Replace(".jpg", ""), kermanImagePath);
        addCustomTraderHelper.SetTraderUpdateTime(traderConfig, traderKerman, timeUtil.GetHoursAsSeconds(1), timeUtil.GetHoursAsSeconds(2));
        addCustomTraderHelper.AddTraderWithEmptyAssortToDb(traderKerman);
        addCustomTraderHelper.AddTraderToLocales(traderKerman, "Mr. Kerman", "???");
        
        await wttcommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly);
    }

    Task IOnLoad.OnLoadAsync(CancellationToken cancellationToken)
    {
        return OnLoadAsync(cancellationToken);
    }
}