using Sokoban;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SokobanApp
{
    public partial class SokobanForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        public const int ElementSize = 32;
        GameState state;

        public SokobanForm()
        {
            var imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);

            state = new GameState();
            ClientSize = new Size(
                ElementSize * state.MapWidth,
                ElementSize * state.MapHeight
            );
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Sokoban";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: state.Move(Direction.left); break;
                case Keys.Right: state.Move(Direction.right); break;
                case Keys.Up: state.Move(Direction.down); break;
                case Keys.Down: state.Move(Direction.up); break;
                case Keys.R: state.Restart(); break;
                default: return;
            }
            if (state.IsOver)
            {
                if (state.HasNextLevel())
                {
                    state.NextLevel();
                    ClientSize = new Size(
                        ElementSize * state.MapWidth,
                        ElementSize * state.MapHeight
                    );
                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(
                    Brushes.Black, 0, 0, ElementSize * state.MapWidth, ElementSize * state.MapHeight
            );
            
            var map = state.Map;
            for (var i = 0; i < state.MapWidth; ++i)
            {
                for (var j = 0; j < state.MapHeight; ++j)
                {
                    var name = map[i, j].GetImageFileName();
                    if (name != null)
                        e.Graphics.DrawImage(
                            bitmaps[name],
                            new Point(i * ElementSize, j * ElementSize)
                        );
                }
            }
            e.Graphics.DrawString(state.CurrentLevel.ToString(), new Font("Arial", 16), Brushes.White, 0, 0);
        }
    }
}
