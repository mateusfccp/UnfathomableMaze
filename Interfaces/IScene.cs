namespace UnfathomableMaze.Interfaces;

/// <summary>
/// A game scene that draws to the screen.
/// </summary>
public interface IScene
{
    /// <summary>
    /// Draws the scene to the screen.
    /// </summary>
    /// <param name="buffer">The buffer to draw onto.</param>
    void Draw(Char[,] buffer);

    /// <summary>
    /// Called when a key is pressed.
    /// </summary>
    /// <param name="key">The key pressed.</param>
    void OnKeyPressed(ConsoleKey key);
}