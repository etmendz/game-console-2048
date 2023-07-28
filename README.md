# GameConsole2048
Inspired by 2048 -- https://github.com/gabrielecirulli/2048/.

GameConsole2048 is a simple console app version of 2048 written in C#.

The game's program flow reads like Play -> Ready -> Set -> Go, which implements the basic game construct:

    if (Start())
    {
        do
        {
            if (Move())
            {
                if (!Continue()) break;
            }
        } while (!GameOver());
    }
    End();

The game UI is designed to align with the basic game construct described above, thus the methods to Start(), Move(), Continue(), GameOver() and End().

The game UX provides the capabilities to support game flow and game play interactions.

The game IO provides the capabilities to support saving and loading game stat and game data (model).

Built-in features include the ability to save and load game stat and game data (model).

Note that the GameConsole2048 project references the GameLibrary2048 project.

## GameLibrary2048
The game grid is designed to align with the basic game construct described above, thus the methods to Start(), Move(), Continue(), GameOver() and End().

The game model is derived by the the game grid, which therefore also represents the game data, that can be used to save and load a game.

The game cell has the NEWS (north, east, west, south) properties, which are references to the game cell's neighbors in the game grid.

Built-in features include the ability to keep track of goal, moves, score, won status, game time and game over state.

Note that the GameLibrary2048 project uses the .NET Community Toolkit High Performance package -- https://learn.microsoft.com/en-us/dotnet/communitytoolkit/high-performance/introduction.

## TestGameLibrary2048
Tests for the core types in GameLibrary2048 are available. This test project is using MSTest.

Coverage is fair with mock up control game data sets to represent common game play scenarios.

## Native AOT
The GameConsole2048 and GameLibrary2048 projects are native AOT compatible/ready.

Source generation contexts are created for game model and game stat, each getting its own game IO implementation. These enhancements are important to support publishing native AOT builds.

Publish profiles (.pubxml) are included for the following Runtime Identifiers (RID):

* For win-x64 (ex. Windows 11): dotnet publish -p:PublishProfile=FolderProfile
* For linux-x64 (ex. WSL+Debian): dotnet publish -p:PublishProfile=FolderProfile1
* For linux-arm64 (ex. Raspberry Pi OS): dotnet publish -p:PublishProfile=FolderProfile2

These can be used as basis/pattern for creating publish profiles that target other RIDs not listed above.

Be sure to run the dotnet publish commands above in the same folder where the GameConsole2048 project is.

Although the GameConsole2048 and GameLibrary2048 projects are native AOT compatible/ready, publishing to a native AOT build is not required.

## Tools
Scripts are provided to help publish native AOT versions for the following RIDs:

* For win-x64 (ex. Windows 11): publish-nativeaot-win-x64.bat
* For linux-x64 (ex. WSL+Debian): publish-nativeaot-linux-x64.sh
* For linux-arm64 (ex. Raspberry Pi OS): publish-nativeaot-linux-arm64.sh

These can be used as basis/pattern for creating publish profiles that target other RIDs not listed above.

Although the GameConsole2048 and GameLibrary2048 projects are native AOT compatible/ready, publishing to a native AOT build is not required.

## Artifacts
Build outputs go to the solution's artifacts\ subdirectory:

    game-console-2048\
        artifacts\
            bin\
                GameConsole2048\
                    debug\
                    release\
                    release_<RID>\*
                GameLibrary2048\
                    debug\
                    release\
                TestGameLibrary2048\
                    debug\*
                    release\*
            obj\
                GameConsole2048\
                    debug\*
                    publish\<RID>\
                    release\*
                    release_<RID>\*
                GameLibrary2048\
                    debug\*
                    release\*
                TestGameLibrary2048\
                    debug\*
                    release\*
            publish\GameConsole2048\release\<RID>
        src\
            GameConsole2048\
                Properties\
                    PublishProfiles\
            GameLibrary2048
        test\
            TestGameLibrary2048\
        tools\

## Known Issues
The ProcessExit event handler calls the methods to save game stat and game data (model). This event handler is called when the player presses [Esc], or clicks on the console/terminal tab/window's close [x] button.

For the native AOT builds though, the ProcessExit event handler is called only when the player presses [Esc]. This event handler is not called when the player clicks on the console/terminal tab/window's close [x] button.
