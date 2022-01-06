using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TearEngine.Engine.Objects;

namespace TearEngine.Engine.Tools
{
    /// <summary>
    /// Batch renderer used for rendering multiple sprites at once
    /// </summary>
    internal class BatchRenderer
    {
        public static bool debug = false;
        public static List<BatchRenderer> debuginstances = new List<BatchRenderer>();

        TearEngineCore EngineCore = TearEngineCore.Instance;

        List<Sprite2D> Sprites = new List<Sprite2D>();

        Sprite2D BatchedSprite;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public BatchRenderer()
        {

        }

        /// <summary>
        /// Queues a sprite for the batch renderer
        /// </summary>
        /// <param name="Sprite"></param>
        public void Queue(Sprite2D Sprite)
        {
            Sprites.Add(Sprite);
        }

        /// <summary>
        /// Compiles and renders, will recompile if called more than one time
        /// </summary>
        /// <returns></returns>
        public Sprite2D Compile()
        {
            if(BatchedSprite != null)
            {
                BatchedSprite.Destroy();
            }

            Direction4 outerBounds = new Direction4();

            outerBounds.Left = Sprites.Select(t => t.X).Min();
            outerBounds.Top = Sprites.Select(t => t.Y).Min();

            outerBounds.Right = Sprites.Select(t => t.X + t.Size.X).Max();
            outerBounds.Bottom = Sprites.Select(t => t.Y + t.Size.Y).Max();

            DebugConsole.Log($"{outerBounds.Right} - {outerBounds.Bottom}");

            Bitmap batchMap = new Bitmap(outerBounds.Right, outerBounds.Bottom - outerBounds.Top);
            Graphics g = Graphics.FromImage(batchMap);
            g.CompositingMode = CompositingMode.SourceOver;

            DebugConsole.Log($"BATCHED: Left-{outerBounds.Left} Top-{outerBounds.Top} Right-{outerBounds.Right} Bottom-{outerBounds.Bottom}");

            foreach (Sprite2D Sprite in Sprites)
            {
                Sprite.Visible = false;
                g.DrawImage(Sprite.TextureBitmap, Sprite.X - outerBounds.Left, Sprite.Y - outerBounds.Top, Sprite.Size.X, Sprite.Size.Y);

                if (debug)
                {
                    g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), Sprite.X - outerBounds.Left, Sprite.Y - outerBounds.Top, Sprite.Size.X, Sprite.Size.Y);
                }
            }

            int index = TearEngineCore.Instance.Sprites.IndexOf(Sprites[0]);
            BatchedSprite = new Sprite2D("", outerBounds.Left, outerBounds.Top, outerBounds.Right - outerBounds.Left, outerBounds.Bottom - outerBounds.Top, Color.White, batchMap, index) { Collidable = false };

            if(!debuginstances.Contains(this))
            {
                debuginstances.Add(this);
            }

            g.Dispose();

            return BatchedSprite;
        }
    }
}