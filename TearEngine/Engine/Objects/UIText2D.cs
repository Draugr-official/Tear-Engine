using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TearEngine.Engine.Tools;

namespace TearEngine.Engine.Objects
{
    /// <summary>
    /// Tear engine text object
    /// </summary>
    internal class UIText2D : Sprite2D
    {
        string _text = "";

        public string Text 
        { 
            get
            {
                return _text;
            } 
            set
            {
                _text = value;

                Size textSize = TextRenderer.MeasureText(value, this.Font);

                this.Size.X = textSize.Width;
                this.Size.Y = textSize.Height;

                Bitmap bitmap = new Bitmap(textSize.Width, textSize.Height);
                Graphics g = Graphics.FromImage(bitmap);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawString(this.Text, this.Font, this.FontColor, X, Y);
                g.Dispose();

                this.Rect.TopLeft = new EVector2(this.X, this.Y);
                this.Rect.TopRight = new EVector2(this.X + this.Size.X, this.Y);
                this.Rect.BottomRight = new EVector2(this.X, this.Y) + this.Size;
                this.Rect.BottomLeft = new EVector2(this.X, this.Y + this.Size.Y);

                Textures.RemoveTexture(this.TextureID);
                this.TextureID = Textures.CreateTexture(bitmap);
            }
        }

        public string FontName { get; set; }
        public int FontSize { get; set; }
        public Font Font { get; set; }
        public Brush FontColor { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fontSize"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public UIText2D(string name, string text, string fontName, int fontSize, Color fontColor, int X, int Y, Color? fallBack = null)
        {
            this.Name = name;
            this.Type = SpriteType.Text2D;
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontColor = new SolidBrush(fontColor);
            this.Font = new Font(fontName, fontSize);
            this.X = X;
            this.Y = Y;

            if(fallBack == null)
            {
                this.Fallback = Color.White;
            }
            else
            {
                this.Fallback = (Color)fallBack;
            }

            this.Text = text;
        }
    }
}
