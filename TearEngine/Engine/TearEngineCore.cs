using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using TearEngine.Engine.Objects;
using TearEngine.Engine.Tools;

namespace TearEngine.Engine
{
    internal class TearEngineCore
    {
        public GameWindow Window { get; set; }

        public static TearEngineCore Instance { get; set; }

        /// <summary>
        /// The background of the output
        /// </summary>
        public Color Background = Color.FromArgb(118, 147, 179);

        Sprite2D Player { get; set; }
        public List<Sprite2D> Sprites = new List<Sprite2D>();

        Thread LogicThread { get; set; }

        EngineStatus Status = EngineStatus.Halted;

        public int FPS = 144;
        public string Title = "Tear Engine";

        public EVector2 CameraPosition = new EVector2();
        EVector2 OldPosition = new EVector2();

        bool DebugMode = false;
        UIText2D FPSDebugLabel;

        /// <summary>
        /// Initialize engine
        /// </summary>
        public TearEngineCore(string Title, int Width, int Height)
        {
            this.Window = new GameWindow(Width, Height, new GraphicsMode(32, 24, 0, 8), Title)
            {
                TargetRenderFrequency = this.FPS,
                TargetUpdateFrequency = this.FPS,
            };
            this.Window.VSync = VSyncMode.Off;

            this.Window.RenderFrame += Window_RenderFrame;
            this.Window.Resize += Window_Resize;
            this.Window.KeyDown += Window_KeyDown;
            this.Window.KeyUp += Window_KeyUp;
            this.Window.Closing += Window_Closing;

            Instance = this;

            DebugConsole.Init();
            InitializeGL();
        }

        void InitializeGL()
        {
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            FPSDebugLabel = new UIText2D("", "FPS 0", "Consolas", 18, Color.White, 0, 0);
        }

        /// <summary>
        /// Creates a window for the engine and starts up
        /// </summary>
        [STAThread]
        public void Start()
        {
            this.Status = EngineStatus.Running;
            this.Window.Run();
        }

        /// <summary>
        /// Cleans up before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Status = EngineStatus.Terminated;
            GL.BindVertexArray(0);
            Environment.Exit(-1);
        }

        /// <summary>
        ///  Actively updating viewport each time the window is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, this.Window.Width, this.Window.Height);

            RenderFrame();
        }

        /// <summary>
        /// Fired from the Window each time the frame needs to be refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            RenderFrame();
        }

        /// <summary>
        /// Keyboard input logging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch(e.Key)
            {
                case OpenTK.Input.Key.Insert:
                    {
                        if(DebugMode)
                        {
                            DebugConsole.Log("Debug mode disabled");

                            BatchRenderer.debug = false;
                            foreach (BatchRenderer renderer in BatchRenderer.debuginstances)
                            {
                                renderer.Compile();
                            }

                            FPSDebugLabel.Visible = false;
                            DebugMode = false;
                        }
                        else
                        {
                            DebugConsole.Log("Debug mode enabled");

                            BatchRenderer.debug = true;
                            foreach (BatchRenderer renderer in BatchRenderer.debuginstances)
                            {
                                renderer.Compile();
                            }

                            FPSDebugLabel.Visible = true;
                            DebugMode = true;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Keyboard input logging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch(e.Key)
            {

            }
        }

        bool _queuetranslate = false;

        /// <summary>
        /// Moves camera to sprite
        /// </summary>
        /// <param name="sprite"></param>
        public void MoveCamera(int X, int Y)
        {
            CameraPosition.X = OldPosition.X - X;
            CameraPosition.Y = OldPosition.Y - Y;

            OldPosition.X = X;
            OldPosition.Y = Y;

            _queuetranslate = true;
        }

        int drawCalls = 0;
        int pastCalls = 0;

        /// <summary>
        /// Renders a frame and displays it to the specifiec viewport
        /// </summary>
        void RenderFrame()
        {
            if (this.Status == EngineStatus.Running)
            {
                this.Window.Title = new Random().Next(10000, 99999).ToString();

                if (this.DebugMode)
                {
                    FPSDebugLabel.Text = "FPS " + this.Window.UpdateFrequency;
                    this.Sprites.Remove(FPSDebugLabel);
                    this.Sprites.Add(FPSDebugLabel);
                }


                GL.Clear(ClearBufferMask.ColorBufferBit);
                GL.ClearColor(Background);

                if (_queuetranslate)
                {
                    GL.Translate(TranslatePointX(CameraPosition.X) + 1, TranslatePointY(CameraPosition.Y) - 1, 0);
                    _queuetranslate = false;
                }

                for (int i = 0; i < this.Sprites.Count; i++)
                {
                    if (this.Sprites[i].Visible)
                    {
                        Sprite2D Sprite = this.Sprites[i];

                        switch (Sprite.Type)
                        {
                            case SpriteType.Sprite2D:
                                {
                                    CreateQuad(Sprite.Fallback,
                                    Sprite.TextureID,
                                    Sprite.Rect.TopLeft,
                                    Sprite.Rect.TopRight,
                                    Sprite.Rect.BottomRight,
                                    Sprite.Rect.BottomLeft);
                                    break;
                                }

                            case SpriteType.Text2D:
                                {
                                    CreateQuad(Sprite.Fallback,
                                    Sprite.TextureID,
                                    Sprite.Rect.TopLeft + CameraPosition,
                                    Sprite.Rect.TopRight + CameraPosition,
                                    Sprite.Rect.BottomRight + CameraPosition,
                                    Sprite.Rect.BottomLeft + CameraPosition);
                                    break;
                                }
                        }

                        drawCalls++;
                    }
                }

                if (drawCalls != pastCalls)
                {
                    Console.WriteLine($"Draw calls: {drawCalls}");
                    pastCalls = drawCalls;
                }

                drawCalls = 0;

                Window.SwapBuffers();
            }
        }

        public void RegisterSprite(Sprite2D sprite)
        {
            this.Sprites.Add(sprite);
        }

        public void InsertSprite(Sprite2D sprite, int index)
        {
            this.Sprites.Insert(index, sprite);
        }
        
        /// <summary>
        /// Creates a quad and prepares it for rendering
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="point4"></param>
        void CreateQuad(Color color, int texture, EVector2 point1, EVector2 point2, EVector2 point3, EVector2 point4)
        {
            if(texture == -1) // No texture
            {
                GL.Color3(color);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            else
            {
                GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
                GL.BindTexture(TextureTarget.Texture2D, texture);
            }

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(TranslatePointX(point1.X), TranslatePointY(point1.Y));

            GL.TexCoord2(1, 0);
            GL.Vertex2(TranslatePointX(point2.X), TranslatePointY(point2.Y));

            GL.TexCoord2(1, 1);
            GL.Vertex2(TranslatePointX(point3.X), TranslatePointY(point3.Y));

            GL.TexCoord2(0, 1);
            GL.Vertex2(TranslatePointX(point4.X), TranslatePointY(point4.Y));

            GL.End();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float TranslatePointX(float point) => point / this.Window.Width - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        float TranslatePointY(float point) => -point / this.Window.Height + 1;
    }
}
