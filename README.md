[Wiki](https://github.com/etmendz/game-console-2048/wiki)
# GameConsole2048
Inspired by 2048 -- https://github.com/gabrielecirulli/2048/.

GameConsole2048 is a simple console app version of 2048 written in C#.

Uses the [GameLibrary](https://github.com/etmendz/game-library/wiki) framework to define the game play, game UI and overall game flow.

Uses the .NET Community Toolkit High Performance package -- https://learn.microsoft.com/en-us/dotnet/communitytoolkit/high-performance/introduction.

## Game UI
The game UI implements IGameUI.

The game UX provides the capabilities to support game play interactions.

The game IO provides the capabilities to support saving and loading game stat and game data (model).

Built-in features include the ability to save and load game stat and game data (model).

## Game Play
The game grid implements IGamePlay.

The game model is derived by the the game grid, which therefore also represents the game data, that can be used to save and load a game.

The game cell has the NEWS (north, east, west, south) properties, which are references to the game cell's neighbors in the game grid.

Built-in features include the ability to keep track of goal, moves, score, won status, game time and game over state.

## Native AOT
The GameConsole2048 and GameLibrary2048 projects are native AOT compatible/ready.

Source generation contexts are created for game model and game stat, each getting its own game IO implementation. These enhancements are important to support publishing native AOT builds.

Publish profiles (.pubxml) are included for the following Runtime Identifiers (RID):

* For win-x64 (ex. Windows 11): dotnet publish -p:PublishProfile=FolderProfile
* For linux-x64 (ex. WSL+Debian): dotnet publish -p:PublishProfile=FolderProfile1
* For linux-arm64 (ex. Raspberry Pi OS): dotnet publish -p:PublishProfile=FolderProfile2

These can be used as basis/pattern for creating publish profiles that target other RIDs not listed above.

Be sure to run the dotnet publish commands above in the same folder where the GameConsole2048 project is.

Although the GameConsole2048 project is native AOT compatible/ready, publishing to a native AOT build is not required.

## Tools
Scripts are provided to help publish native AOT versions for the following RIDs:

* For win-x64 (ex. Windows 11): publish-nativeaot-win-x64.bat
* For linux-x64 (ex. WSL+Debian): publish-nativeaot-linux-x64.sh
* For linux-arm64 (ex. Raspberry Pi OS): publish-nativeaot-linux-arm64.sh

These can be used as basis/pattern for creating publish scripts that target other RIDs not listed above.

Although the GameConsole2048 project is native AOT compatible/ready, publishing to a native AOT build is not required.

## Artifacts
Build outputs go to the solution's artifacts\ subdirectory:

    game-console-2048\
        artifacts\
            bin\
                GameConsole2048\
                    debug\
                    release\
                    release_<RID>\*
            obj\
                GameConsole2048\
                    debug\*
                    publish\<RID>\
                    release\*
                    release_<RID>\*
            publish\GameConsole2048\release\<RID>
        src\
            GameConsole2048\
                Properties\PublishProfiles\
        tools\

## Known Issues
The ProcessExit event handler calls the methods to save game stat and game data (model). This event handler is called when the player presses [Esc], or clicks on the console/terminal tab/window's close [x] button.

For the native AOT builds though, the ProcessExit event handler is called only when the player presses [Esc]. This event handler is not called when the player clicks on the console/terminal tab/window's close [x] button.

---

(c) Mendz, etmendz. All rights reserved.
