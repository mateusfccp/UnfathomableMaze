using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes
{
    public class DeathScene : IScene
    {
        public void Draw(Engine.Canvas canvas)
        {
            canvas.Clear();
            canvas.Draw("Ur ded lol", 1, 1);
            canvas.Draw("Press", 1, 2);
            canvas.Draw("Esc", 7, 2, new Models.Style(Color.Salmon));
            canvas.Draw("to return to Menu", 11, 2);
        }

        public void OnKeyPressed(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape) { Engine.Instance?.UpdateScene(new MenuScene()); }
        }
    }
}
