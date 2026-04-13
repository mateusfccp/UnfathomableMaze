using UnfathomableMaze.Services;

namespace UnfathomableMaze.Interfaces;

/// <summary>
/// A game scene that draws to the screen.
/// </summary>
public interface IScene
{
    /// <summary>
    /// Draws the scene to the screen.
    /// </summary>
    /// <param name="canvas">A canvas to draw onto.</param>
    void Draw(GameEngine.Canvas canvas);

    /// <summary>
    /// Called when a key is pressed.
    /// </summary>
    /// <param name="key">The key pressed.</param>
    void OnKeyPressed(ConsoleKey key);
}