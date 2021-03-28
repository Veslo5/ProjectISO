# ProjectISO


- **ISO.CORE** - engine core.
- **ISO.DB.CommandLineManager** - CLM manager for basic work with metadata datbase without external editor.
- **ProjectISO** - game implementation on engine core.

## File extensions
- *.DAT - sqlite database
- *.ZIP - packed game resources
- *.XNB - MGCB compiled resources (in ZIP)
- *.UI - json UI file
- *.METAMAP - json with tiled map metadata
- *.METAPIC - json with tileset informations used in metamap
- *.SCRIPT - lua script for game logic

## Core providers, managers, controllers
| Name  | Info | Type | Access level |
| ------------- | ------------- | ------------- | ------------- |
|  CorountineManager | Providing management to corountine fuctions | Manager | Logic |
|  LuaManager | Providing management for lua script fuctions | Manager | Logic |
|  ISOTiledManager | Providing management for tiled maps| Manager | Logic |
|  UIManager | Providing management for UI | Manager | Logic |
|  InputController | Providing game input | Controller | Logic |
|  SceneController | Providing scenes control | Controller | Logic |
|  LoadingController | Providing loading functions | Controller | Logic |
|  ISOContentProvider | Extending ContentManager | Provider | Internal functions |
|  ISOGraphicProvider | Extending GraphicsDeviceManager | Provider | Internal functions |

- Managers - Could be used from LUA scripting (unique for each level)
- Controllers - On game top level (can be accessed from anywhere)
- Providers - Internal components which extending monogame functions (should not be touched from scripts) 

## File formats

| Value  | Example |
| ------------- | ------------- |
| **Location:** [locationID]Name  | **Example:** 1 "Alpha" |
| **Map:** [mapID]Name  | **Example:** 0 "Forest" |
| **Map_Data:** [MapID][Name].METAMAP or [locationID][Name].METAPIC | **Example:** - 0FOREST.METAMAP - 0FORESTTILES.METAPIC |
| **Script:** [MapID][Name].SCRIPT  |  **Example:** 0JUMP.SCRIPT | 
| **UI:** [MapID][Name].UI |  **Example:** 0MAIN.UI | 


## External Editors
- Map editor: Tiled [https://www.mapeditor.org/]
- Content compiler: MGCB [https://docs.monogame.net/articles/tools/mgcb.html]
- Time manager: Trello [https://trello.com/b/97w6VwVH/isometric-engine-monogame-road-to-alpha]
- PIXI particles editor: [https://pixijs.io/pixi-particles-editor/]

## Dependencies:
#### Core & ProjectISO
- .NET CORE 3.1 [https://dotnet.microsoft.com/]
- sqlite-net [https://github.com/praeclarum/sqlite-net]
- Newtonsoft.Json [https://github.com/JamesNK/Newtonsoft.Json]
- Monogame 3.8 [https://www.monogame.net/]
- moonsharp [https://github.com/moonsharp-devs/moonsharp]

#### CLM
- commandlineparser [https://github.com/commandlineparser/commandline

#### Future :)
- Pixi [https://github.com/playerthree-ltd/MonoGame-Pixi-Particles]
