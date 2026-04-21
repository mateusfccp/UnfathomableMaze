using System.Drawing;
using UnfathomableMaze.Interfaces;
using UnfathomableMaze.Services;

namespace UnfathomableMaze.Scenes
{
    public class DeathScene : IScene
    {
        private int _score;
        private long _steps;

        public DeathScene(int score, long steps)
        {
            _score = score;
            _steps = steps;
        }

        public void Draw(Engine.Canvas canvas)
        {
            canvas.Clear();

            int centerOffsetX = canvas.Width / 2;
            int centerOffsetY = canvas.Height / 2;

            // Score
            canvas.Draw($"Puntaje: {_score}", 1, 1);
            // Steps
            canvas.Draw($"Pasos: {_steps}", 1, 2);

            canvas.Draw("  ▄▄▄▄███▄▄▄▄    ▄██████▄     ▄████████  ▄█     ▄████████     ███        ▄████████  ", centerOffsetX - 42, centerOffsetY - 8);
            canvas.Draw(" ▄██▀▀▀███▀▀▀██▄ ███    ███   ███    ███ ███    ███    ███ ▀█████████▄   ███    ███ ", centerOffsetX - 42, centerOffsetY - 7);
            canvas.Draw(" ███   ███   ███ ███    ███   ███    ███ ███▌   ███    █▀     ▀███▀▀██   ███    █▀  ", centerOffsetX - 42, centerOffsetY - 6);
            canvas.Draw(" ███   ███   ███ ███    ███  ▄███▄▄▄▄██▀ ███▌   ███            ███   ▀  ▄███▄▄▄     ", centerOffsetX - 42, centerOffsetY - 5);
            canvas.Draw(" ███   ███   ███ ███    ███ ▀▀███▀▀▀▀▀   ███▌ ▀███████████     ███     ▀▀███▀▀▀     ", centerOffsetX - 42, centerOffsetY - 4);
            canvas.Draw(" ███   ███   ███ ███    ███ ▀███████████ ███           ███     ███       ███    █▄  ", centerOffsetX - 42, centerOffsetY - 3);
            canvas.Draw(" ███   ███   ███ ███    ███   ███    ███ ███     ▄█    ███     ███       ███    ███ ", centerOffsetX - 42, centerOffsetY - 2);
            canvas.Draw("  ▀█   ███   █▀   ▀██████▀    ███    ███ █▀    ▄████████▀     ▄████▀     ██████████ ", centerOffsetX - 42, centerOffsetY - 1);
            canvas.Draw("                              ███    ███                                            ", centerOffsetX - 42, centerOffsetY);
            canvas.Draw("Presione", centerOffsetX - 15, centerOffsetY + 3);
            canvas.Draw("Esc", centerOffsetX - 6, centerOffsetY + 3, new Models.Style(Color.Salmon));
            canvas.Draw("para volver al Menú", centerOffsetX - 2, centerOffsetY + 3);
        }

        public void OnKeyPressed(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape) { Engine.Instance?.UpdateScene(new MenuScene()); }
        }
    }
}
