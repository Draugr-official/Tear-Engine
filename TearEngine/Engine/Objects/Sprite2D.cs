using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TearEngine.Engine.Tools;

namespace TearEngine.Engine.Objects
{
    class Sprite2D
    {
        /// <summary>
        /// Name of the sprite
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of sprite, used by the renderer and should be left alone
        /// </summary>
        public SpriteType Type = SpriteType.Sprite2D;

        /// <summary>
        /// The registered texture id of the texture for the sprite
        /// </summary>
        public int TextureID = -1;

        /// <summary>
        /// The bitmap of the texture
        /// </summary>
        public Bitmap TextureBitmap { get; set; }

        /// <summary>
        /// Frames for animations
        /// </summary>
        Dictionary<string, int> Frames = new Dictionary<string, int>();

        /// <summary>
        /// Determines if the sprite should be rendered through conventional means
        /// </summary>
        public bool Visible = true;

        /// <summary>
        /// Determines if the sprite is collidable
        /// </summary>
        public bool Collidable = true;

        /// <summary>
        /// Fallback color in case textures are not loading
        /// </summary>
        public Color Fallback { get; set; }

        /// <summary>
        /// Color mask for textured sprites
        /// </summary>
        public Color Mask { get; set; }

        /// <summary>
        /// Directional velocity of the sprite
        /// </summary>
        public Direction4 Velocity = new Direction4(0, 0, 0, 0);

        /// <summary>
        /// The rotation of the sprite in degrees
        /// </summary>
        public int Rotation = 0;

        /// <summary>
        /// Lower variant of Pos+Size, used for better rendering
        /// </summary>
        public Vertex4 Rect = new Vertex4();

        /// <summary>
        /// Transformed variant of rect, used for easier use of transformations
        /// </summary>
        public Vertex4 TransformedRect = new Vertex4();

        private EVector2 _position = new EVector2();
        /// <summary>
        /// Position of the sprite along the X axis
        /// </summary>
        public int X 
        { 
            get
            {
                return _position.X;
            }

            set
            {

                int newPosition = value;

                int incX = newPosition - _position.X;

                Rect.TopLeft.X += incX;
                Rect.TopRight.X += incX;
                Rect.BottomRight.X += incX;
                Rect.BottomLeft.X += incX;

                _position.X = newPosition;
            }
        }

        /// <summary>
        /// Position of the sprite along the Y axis
        /// </summary>
        public int Y 
        { 
            get 
            { 
                return _position.Y; 
            } 

            set
            {
                int newPosition = value;

                int incY = newPosition - _position.Y;

                Rect.TopLeft.Y += incY;
                Rect.TopRight.Y += incY;
                Rect.BottomRight.Y += incY;
                Rect.BottomLeft.Y += incY;

                _position.Y = newPosition;
            }
        }

        public EVector2 _size = new EVector2();

        /// <summary>
        /// Size of sprite in pixels
        /// </summary>
        public EVector2 Size
        {
            get 
            {
                return _size; 
            }

            set 
            {
                EVector2 newSize = value;

                int incX = _size.X - newSize.X;
                int incY = _size.Y - newSize.Y;

                Rect.TopRight.X += incX;

                Rect.BottomRight.Y += incY;

                Rect.BottomLeft.X += incX;
                Rect.BottomLeft.Y += incY;

                _size = newSize;
            }
        }

        /// <summary>
        /// Creates a new Sprite2D instance
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Position"></param>
        /// <param name="Size"></param>
        /// <param name="Fallback"></param>
        public Sprite2D(string Name, int X, int Y, int Width, int Height, Color Fallback, Bitmap Texture = null, int index = -1)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Size = new EVector2(Width, Height);
            this.Fallback = Fallback;
            this.TextureBitmap = Texture;
            this.TextureID = Texture == null ? -1 : Textures.CreateTexture(Texture);

            this.Rect.TopLeft = new EVector2(this.X, this.Y);
            this.Rect.TopRight = new EVector2(this.X + this.Size.X, this.Y);
            this.Rect.BottomRight = new EVector2(this.X, this.Y) + this.Size;
            this.Rect.BottomLeft = new EVector2(this.X, this.Y + this.Size.Y);

            Bitmap debugBm = new Bitmap(Texture);
            Graphics graphics = Graphics.FromImage(debugBm);

            graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), 0, 0, debugBm.Width, debugBm.Height);

            AddFrame("debug", debugBm);
            AddFrame("standard", Texture);

            if (TearEngineCore.Instance != null)
            {
                if(index == -1)
                {
                    TearEngineCore.Instance.RegisterSprite(this);
                }
                else
                {
                    TearEngineCore.Instance.InsertSprite(this, index);
                }
            }
        }

        public Sprite2D(string Name, int X, int Y, int Width, int Height, Color Fallback, string TexturePath = null)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Size = new EVector2(Width, Height);
            this.Fallback = Fallback;

            Bitmap bm = new Bitmap(TexturePath);
            this.TextureBitmap = bm;
            this.TextureID = TexturePath == null ? -1 : Textures.CreateTexture(bm);

            this.Rect.TopLeft = new EVector2(this.X, this.Y);
            this.Rect.TopRight = new EVector2(this.X + this.Size.X, this.Y);
            this.Rect.BottomRight = new EVector2(this.X, this.Y) + this.Size;
            this.Rect.BottomLeft = new EVector2(this.X, this.Y + this.Size.Y);

            Bitmap debugBm = new Bitmap(TexturePath);
            Graphics graphics = Graphics.FromImage(debugBm);

            graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), 0, 0, debugBm.Width, debugBm.Height);

            AddFrame("debug", debugBm);
            AddFrame("standard", bm);

            if (TearEngineCore.Instance != null)
            {
                TearEngineCore.Instance.RegisterSprite(this);
            }
        }

        public Sprite2D()
        {
            if (TearEngineCore.Instance != null)
            {
                TearEngineCore.Instance.RegisterSprite(this);
            }
        }

        /// <summary>
        /// Adds a frame to the internal frame cache
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public void AddFrame(string name, string path)
        {
            this.Frames.Add(name, Textures.CreateTexture(new Bitmap(path)));
        }

        /// <summary>
        /// Adds a frame to the internal frame cache
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public void AddFrame(string name, Bitmap bitmap)
        {
            this.Frames.Add(name, Textures.CreateTexture(bitmap));
        }

        /// <summary>
        /// Removes a frame from the internal frame cache
        /// </summary>
        /// <param name="name"></param>
        public void RemoveFrame(string name)
        {
            this.Frames.Remove(name);
        }

        /// <summary>
        /// Displays the given frame as the texture for the sprite
        /// </summary>
        /// <param name="name"></param>
        public void DisplayFrame(string name)
        {
            this.TextureID = this.Frames[name];
        }

        public bool AnimationPlaying = false;

        /// <summary>
        /// Plays a sequence of frames with the desired interval in milliseconds in a specific group (group being the name before a number, ex: 'run1' where 'run' would be the group)
        /// </summary>
        /// <param name="group"></param>
        /// <param name="interval"></param>
        public async void StartAnimation(string group, int interval, bool loop)
        {
            AnimationPlaying = true;
            KeyValuePair<string, int>[] frameGroup = this.Frames.Where(t => t.Key.StartsWith(group)).ToArray();
            DebugConsole.Log($"animation sequence started [{group}]");

            while (loop)
            {
                foreach (KeyValuePair<string, int> frame in frameGroup)
                {
                    if (!AnimationPlaying)
                    {
                        return;
                    }

                    int bitmap = frame.Value;
                    DisplayFrame(frame.Key);

                    await Task.Delay(interval);
                }
            }
        }

        public void StopAnimations()
        {
            DebugConsole.Log("animation sequence stopped");
            AnimationPlaying = false;
        }

        /// <summary>
        /// Rotates the sprite to an angle around an anchor point
        /// </summary>
        /// <param name="Angle"></param>
        public void Rotate(int Angle)
        {
            EVector2 Center = new EVector2(this.X + this.Size.X/2, this.Y + this.Size.Y/2);

            this.Rect.TopLeft = RotatePoint(this.Rect.TopLeft, Center, Angle);
            this.Rect.BottomRight = RotatePoint(this.Rect.BottomRight, Center, Angle);
            this.Rect.BottomLeft = RotatePoint(this.Rect.BottomLeft, Center, Angle);
            this.Rect.TopRight = RotatePoint(this.Rect.TopRight, Center, Angle);

            this.Rotation = Angle;
        }

        public EVector2 RotatePoint(EVector2 ToRotate, EVector2 Anchor, double angle)
        {
            double radians = ConvertToRadians(angle);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            ToRotate.X -= Anchor.X;
            ToRotate.Y -= Anchor.Y;

            double xInc = ToRotate.X * cos - ToRotate.Y * sin;
            double yInc = ToRotate.X * sin + ToRotate.Y * cos;

            return new EVector2((int)xInc + Anchor.X, (int)yInc + Anchor.Y);
        }

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public void Destroy()
        {
            TearEngineCore.Instance.Sprites.Remove(this);
        }
    }
}
