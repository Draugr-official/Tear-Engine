using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TearEngine.Engine;
using TearEngine.Engine.Objects;
using TearEngine.Engine.Tools;

namespace TearEngine.Games
{
    class Game
    {
        TearEngineCore EngineCore = new TearEngineCore("The Game!", 900, 700);

        // Map fields
        Sprite2D[,,] CurrentMap { get; set; }

        // Player fields
        public Sprite2D Player { get; private set; }
        bool MoveLeft = false;
        bool MoveRight = false;
        bool LookingRight = true;

        int MaxSpeed = 11;

        bool PlayerGrounded = false;

        /// <summary>
        /// Starts up the game
        /// </summary>
        public void Run()
        {
            EngineCore.Window.KeyDown += Window_KeyDown;
            EngineCore.Window.KeyUp += Window_KeyUp;

            RenderBackground(1);
            RenderBackground(2);

            this.Player = new Sprite2D("MainPlayer", 100, 100, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Entities/Humanoids/HatBoy/Idle (1).png") { Collidable = false };

            this.Player.AddFrame("jumpr1", "Sprites/Entities/Humanoids/HatBoy/Jump (8).png");
            this.Player.AddFrame("jumpl1", "Sprites/Entities/Humanoids/HatBoy/JumpL (8).png");

            this.Player.AddFrame("idler1", "Sprites/Entities/Humanoids/HatBoy/Idle (1).png");
            this.Player.AddFrame("idlel1", "Sprites/Entities/Humanoids/HatBoy/IdleL (1).png");

            this.Player.AddFrame("runr1", "Sprites/Entities/Humanoids/HatBoy/Run (2).png");
            this.Player.AddFrame("runr2", "Sprites/Entities/Humanoids/HatBoy/Run (3).png");
            this.Player.AddFrame("runr3", "Sprites/Entities/Humanoids/HatBoy/Run (4).png");
            this.Player.AddFrame("runr4", "Sprites/Entities/Humanoids/HatBoy/Run (5).png");
            this.Player.AddFrame("runr5", "Sprites/Entities/Humanoids/HatBoy/Run (6).png");
            this.Player.AddFrame("runr6", "Sprites/Entities/Humanoids/HatBoy/Run (7).png");
            this.Player.AddFrame("runr7", "Sprites/Entities/Humanoids/HatBoy/Run (8).png");

            this.Player.AddFrame("runl1", "Sprites/Entities/Humanoids/HatBoy/RunL (2).png");
            this.Player.AddFrame("runl2", "Sprites/Entities/Humanoids/HatBoy/RunL (3).png");
            this.Player.AddFrame("runl3", "Sprites/Entities/Humanoids/HatBoy/RunL (4).png");
            this.Player.AddFrame("runl4", "Sprites/Entities/Humanoids/HatBoy/RunL (5).png");
            this.Player.AddFrame("runl5", "Sprites/Entities/Humanoids/HatBoy/RunL (6).png");
            this.Player.AddFrame("runl6", "Sprites/Entities/Humanoids/HatBoy/RunL (7).png");
            this.Player.AddFrame("runl7", "Sprites/Entities/Humanoids/HatBoy/RunL (8).png");

            LoadMap("level 1");

            new Thread(() => Physics()).Start();

            EngineCore.Start();
        }

        void RenderBackground(int pos)
        {
            BatchRenderer batchRenderer = new BatchRenderer();

            int tP = EngineCore.Window.Width * 2;
            int tX = tP * pos;

            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 400, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/Layer_0009_2.png") { Collidable = false });
            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 250, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/trunks_l4.png") { Collidable = false });

            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 400, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/light_1.png") { Collidable = false });

            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 300, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/trunks_l3.png") { Collidable = false });
            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 350, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/trunks_l2.png") { Collidable = false });

            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 400, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/light_2.png") { Collidable = false });

            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 400, tX, EngineCore.Window.Height, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/trunks_l1.png") { Collidable = false });
            batchRenderer.Queue(new Sprite2D("Background", tX - tP, 0, tX, 450, Color.AliceBlue, "Sprites/Tiles/Background/dark_forest/leaves.png") { Collidable = false });

            batchRenderer.Compile();
        }

        private void Window_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.A:
                    {
                        if (this.MoveLeft)
                        {
                            this.Player.StopAnimations();
                            this.Player.DisplayFrame("idlel1");
                        }

                        this.MoveLeft = false;
                        break;
                    }

                case OpenTK.Input.Key.D:
                    {
                        if (this.MoveRight)
                        {
                            this.Player.StopAnimations();
                            this.Player.DisplayFrame("idler1");
                        }

                        this.MoveRight = false;
                        break;
                    }
            }
        }

        private void Window_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case OpenTK.Input.Key.A:
                    {
                        if (!this.MoveLeft && !this.MoveRight)
                        {
                            this.Player.StartAnimation("runl", 30, true);
                        }

                        this.MoveLeft = true;
                        this.LookingRight = false;
                        break;
                    }

                case OpenTK.Input.Key.D:
                    {
                        if(!this.MoveRight && !this.MoveLeft)
                        {
                            this.Player.StartAnimation("runr", 30, true);
                        }

                        this.MoveRight = true;
                        this.LookingRight = true;
                        break;
                    }

                case OpenTK.Input.Key.Space:
                    {
                        if (PlayerGrounded)
                        {
                            this.Player.StopAnimations();
                            this.Player.Velocity.Bottom -= 30;
                        }
                        break;
                    }

                case OpenTK.Input.Key.R:
                    {
                        this.Player.X = 100;
                        this.Player.Y = 100;

                        this.Player.Velocity.Bottom = 0;
                        this.Player.Velocity.Right = 0;
                        this.Player.Velocity.Left = 0;
                        this.Player.Velocity.Top = 0;

                        DebugConsole.Log("Reset character");

                        break;
                    }
            }
        }

        /// <summary>
        /// Game physics thread
        /// </summary>
        void Physics()
        {
            bool playerAirborne = false;

            for (; ; )
            {
                this.Player.Velocity.Bottom += 2;

                Sprite2D TileBelow = TileOccupied(this.Player.X + (this.Player.Size.X / 2), this.Player.Y + this.Player.Size.Y + this.Player.Velocity.Bottom + 10);
                if (TileBelow != null)
                {

                    this.Player.Velocity.Bottom = 0;
                    this.Player.Y = TileBelow.Y - this.Player.Size.Y;

                    if(playerAirborne)
                    {
                        if(this.MoveRight)
                        {
                            if(!this.Player.AnimationPlaying)
                            {
                                this.Player.StartAnimation("runr", 30, true);
                            }
                        }
                        else if(this.MoveLeft)
                        {
                            if (!this.Player.AnimationPlaying)
                            {
                                this.Player.StartAnimation("runl", 30, true);
                            }
                        }

                        playerAirborne = false;
                    }

                    PlayerGrounded = true;
                }
                else
                {
                    if (!playerAirborne)
                    {
                        if(this.MoveRight)
                        {
                            this.Player.DisplayFrame("jumpr1");
                        }
                        if(this.MoveLeft)
                        {
                            this.Player.DisplayFrame("jumpl1");
                        }
                        playerAirborne = true;
                    }

                    PlayerGrounded = false;
                }

                this.Player.Y += this.Player.Velocity.Bottom;

                if(MoveLeft && this.Player.Velocity.Left < MaxSpeed)
                {
                    this.Player.Velocity.Left += 2;
                }

                if(MoveRight && this.Player.Velocity.Right < MaxSpeed)
                {
                    this.Player.Velocity.Right += 2;
                }

                this.Player.Velocity.Left -= (this.Player.Velocity.Left > 0 ? 1 : 0);
                this.Player.Velocity.Right -= (this.Player.Velocity.Right > 0 ? 1 : 0);

                this.Player.X += -this.Player.Velocity.Left + this.Player.Velocity.Right;

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Fetches instances at tile
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        Sprite2D TileOccupied(int X, int Y)
        {
            for(int i = 0; i < EngineCore.Sprites.Count; i++)
            {
                Sprite2D sprite = EngineCore.Sprites[i];
                if(sprite.Collidable && sprite.X < X && sprite.Y < Y && sprite.X + sprite.Size.X > X && sprite.Y + sprite.Size.Y > Y)
                {
                    return sprite;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads given map using the tear engine batch renderer
        /// </summary>
        /// <param name="name"></param>
        void LoadMap(string name)
        {
            BatchRenderer batchRenderer = new BatchRenderer();
            switch (name)
            {
                case "level 1":
                    {
                        LoadChunk(batchRenderer, Maps.Chunk1, 0);
                        LoadChunk(batchRenderer, Maps.Chunk2, 1);
                        LoadChunk(batchRenderer, Maps.Chunk2, 2);
                        break;
                    }
            }
            batchRenderer.Compile();
        }

        /// <summary>
        /// Loads a chunk into the batch renderer
        /// </summary>
        /// <param name="batchRenderer"></param>
        /// <param name="Map"></param>
        /// <param name="chunkNum"></param>
        void LoadChunk(BatchRenderer batchRenderer, string[,] Map, int chunkNum)
        {
            CurrentMap = new Sprite2D[Map.GetLength(0), Map.GetLength(1), 15];

            for (int x = 0; x < Map.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < Map.GetLength(1) - 1; y++)
                {
                    int Base = 0;
                    int tX = x * 100 + ((Map.GetLength(0) * 100) * chunkNum) - (chunkNum > 0 ? chunkNum * 100 : 0);
                    int tY = y * 100;

                    if (Map[y, x].Contains("GroundUp"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundUp", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/2.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }

                    if (Map[y, x].Contains("GroundNone"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundNone", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/5.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }

                    if (Map[y, x].Contains("GroundRight"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundNone", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/6.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }

                    if (Map[y, x].Contains("GroundLeft"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundNone", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/4.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }

                    if (Map[y, x].Contains("GroundRightEdgeTop"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundNone", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/3.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }

                    if (Map[y, x].Contains("GroundLeftEdgeTop"))
                    {
                        Sprite2D Tile = CurrentMap[y, x, Base] = new Sprite2D("GroundNone", tX, tY, 100, 100, Color.FromArgb(100, 200, 100), "Sprites/Tiles/1.png");
                        Tile.Visible = false;
                        batchRenderer.Queue(Tile);

                        Base += 1;
                    }
                }
            }
        }
    }
}