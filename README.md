# TerminalCommander

> Adds a custom trader and in-raid questline related to Terminal.

**Author:** mizmii
**Version:** 1.0.0
**SPT Version:** 4.1.2
**License:** MIT

---

## What This Mod Does

Describe what your mod does here. Be specific about game systems it modifies.

---

## Requirements

- [SPT](https://www.sp-tarkov.com/) **4.1.2** or compatible
- .NET 9 SDK (for building from source)

---

## Building

```sh
git clone 
cd TerminalCommander
dotnet build -c Release
```

The build target automatically packages the mod into a distributable `TerminalCommander.zip`.

---

## Installation

1. Build the project (see above) **or** download the latest release zip.
2. Extract the zip so that `TerminalCommander.dll` ends up in:
   ```
   <SPT root>/user/mods/TerminalCommander/
   ```
3. Launch SPT server as usual.

---

## Configuration

No configuration file is required by default. Extend `Mod.cs` to add your own settings.

---

## Project Structure

```
TerminalCommander/
├── TerminalCommander.csproj   ← project file + packaging target
├── Mod.cs                ← entry point (ModMetadata + IOnLoad)
├── README.md
└── .gitignore
```

---

## Learning Resources

| Resource | URL |
|---|---|
| SPT Server (C#) — Overview | https://deepwiki.com/sp-tarkov/server-csharp/1-overview |
| Server Mod Examples | https://github.com/sp-tarkov/server-mod-examples |
| SPT Wiki Modding Resources | https://wiki.sp-tarkov.com/modding/Modding_Resources |
| SPT Client Mod Examples | https://github.com/Jehree/SPTClientModExamples |

---

