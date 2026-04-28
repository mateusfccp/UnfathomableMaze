# Unfathomable Maze
[![Ask DeepWiki](https://devin.ai/assets/askdeepwiki.png)](https://deepwiki.com/mateusfccp/UnfathomableMaze)

Unfathomable Maze is a console-based maze game written in C# and .NET. It features procedurally generated mazes, a
custom rendering engine for rich console output, and multiple game scenes. Navigate through an ever-changing labyrinth,
track your score, and challenge yourself in different modes.

# **IMPORTANT**
- Before running the program **make sure** to download the monospaced font and apply it to the CLI you are using. This ensures that the maze and tables look good.
- The monospaced font can be found in the files of the repository as "UnfathomableFont.ttf" or you can also download it [**HERE**](https://www.mediafire.com/file/am551b3hgncdymn/UnfathomableFont.ttf/file).
- After downloading it just go to your CLI config and select the font u just downloaded.
- You can also use other fonts of your liking as long as they are monospaced.

## Features

* **Procedurally Generated Mazes**: Every maze is unique, generated using a randomized depth-first search algorithm.
* **Two Game Modes**:
    * **Easy Mode**: Explore the maze freely. Walls will block your path.
    * **Hard Mode**: A single mistake is fatal. Colliding with a wall ends the game.
* **Infinite Levels**: Upon reaching the finish, a new, larger maze is generated, and your score increases.
* **Console Rendering Engine**: A custom engine handles drawing to the console, using a double-buffer technique to
  minimize flicker and provide smooth updates.
* **ANSI Styling**: The game uses ANSI escape codes for 24-bit color and text decorations like bold and inverted colors,
  creating a visually rich experience in a modern terminal.
* **Interactive Menu**: A simple menu allows you to select your game mode or view a demonstration table.

## How to Play

* Use the **Arrow Keys** (<kbd>↑</kbd>, <kbd>↓</kbd>, <kbd>←</kbd>, <kbd>→</kbd>) to move your character (`@`).
* Your goal is to reach the finish flag (`⚿`) located at the bottom-right corner of the maze.
* Each completed maze increases your score and generates a new, potentially larger maze.
* In **Hard Mode**, running into a wall will end the game.
* Press <kbd>Esc</kbd> at any time from the maze or table scene to return to the main menu.

## Getting Started

### Prerequisites

* [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or a compatible version.

### Running the Application

1. Clone the repository:
    ```sh
    git clone https://github.com/mateusfccp/UnfathomableMaze.git
    ```
2. Navigate to the project directory:
    ```sh
    cd UnfathomableMaze
    ```
3. Run the application:
    ```sh
    dotnet run
    ```

## Project Structure

* `Program.cs`: The entry point of the application, which initializes and starts the game engine with the `MenuScene`.
* `Services/Engine.cs`: The core game loop and rendering engine. It manages scenes, processes user input, and draws to
  the console using an efficient double-buffering system. The `Canvas` inner class provides a drawing surface for 
  scenes.
* `Services/MapGenerator.cs`: Responsible for generating the maze structure. It uses a depth-first search algorithm to
  carve paths and can add loops to create more complex, braided mazes.
* `Services/ANSI.cs`: A utility class for generating ANSI escape code sequences to control cursor position, colors, and
  text styles in the console.
* `Scenes/`: Contains the logic for different screens in the game.
    * `MenuScene.cs`: The main menu screen where the player selects a game mode.
    * `MazeScene.cs`: The primary gameplay scene. It handles player movement, game state validation (winning or losing),
      and rendering the maze, player, and UI elements.
    * `TableScene.cs`: A demonstration scene that draws a formatted, styled table in the console, showcasing the
      rendering engine's capabilities.
    * `DeathScene.cs`: The "Game Over" screen shown in hard mode.
* `Controllers/MazeController.cs`: Manages the maze data and provides logic for validating movement against the maze
  walls.
* `Interfaces/`: Defines contracts for key components.
    * `IScene.cs`: The interface for all game scenes, ensuring they implement `Draw` and `OnKeyPressed` methods.
    * `IMapTilesGenerator.cs`: The interface for maze generation services.
* `Enums/`: Contains enumerations used throughout the project, such as `Tile` (Wall/Path) and `Decoration` (Bold,
  Invert, etc.).
* `Models/`: Contains data structures, such as `Style`, which defines the visual appearance (color, decoration) of a
  character.
