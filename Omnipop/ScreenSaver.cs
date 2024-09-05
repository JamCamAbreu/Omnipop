using HPScreen.Admin;
using HPScreen.Entities;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Omnipop.Admin;
using Omnipop.Entities;
using SharpDX.Direct2D1;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;

namespace HPScreen
{
    public class ScreenSaver : Game
    {
        private GraphicsDeviceManager _graphics;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private int loadFrames = 0;
        private const int LOAD_FRAMES_THRESH = 10;
        Effect circleCropEffect;

        public static List<Head> Heads { get; set; } = new List<Head>();
        protected List<string> HeadNames { get; set; } = new List<string>();
        public Turret GameTurret { get; set; }

        protected bool RunSetup { get; set; }
        public ScreenSaver()
        {
            Graphics.Current.GraphicsDM = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            // Set the following property to false to ensure alternating Step() and Draw() functions
            // Set the property to true to (hopefully) improve game smoothness by ignoring some draw calls if needed.
            IsFixedTimeStep = false;

            RunSetup = true;
        }
        protected override void Initialize()
        {
            // Note: This takes places BEFORE LoadContent()
            Graphics.Current.Init(this.GraphicsDevice, this.Window, true);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Graphics.Current.SpriteB = new Microsoft.Xna.Framework.Graphics.SpriteBatch(Graphics.Current.GraphicsDM.GraphicsDevice);

            Graphics.Current.SpritesByName.Add("turret", Content.Load<Texture2D>($"Sprites/misc/Turret"));
            Graphics.Current.SpritesByName.Add("turretbarrel", Content.Load<Texture2D>($"Sprites/misc/TurretBarrel"));
            Graphics.Current.SpritesByName.Add("ball", Content.Load<Texture2D>($"Sprites/misc/Ball"));

            HeadNames.Add("aaron");
            HeadNames.Add("acole");
            HeadNames.Add("aj");
            HeadNames.Add("alej");
            HeadNames.Add("allison");
            HeadNames.Add("brett");
            HeadNames.Add("brian");
            HeadNames.Add("cam");
            HeadNames.Add("chad");
            HeadNames.Add("chris");
            HeadNames.Add("dakota");
            HeadNames.Add("dalton");
            HeadNames.Add("daryl");
            HeadNames.Add("denise");
            HeadNames.Add("dustin");
            HeadNames.Add("dusty");
            HeadNames.Add("emily");
            HeadNames.Add("isaac");
            HeadNames.Add("jack");
            HeadNames.Add("jacob");
            HeadNames.Add("jackadam");
            HeadNames.Add("jeff");
            HeadNames.Add("john");
            HeadNames.Add("jonah");
            HeadNames.Add("josh");
            HeadNames.Add("jpaul");
            HeadNames.Add("juan");
            HeadNames.Add("katelyn");
            HeadNames.Add("keegan");
            HeadNames.Add("keiran");
            HeadNames.Add("kenny");
            HeadNames.Add("kevin");
            HeadNames.Add("mary");
            HeadNames.Add("max");
            HeadNames.Add("michael");
            HeadNames.Add("nicole");
            HeadNames.Add("noah");
            HeadNames.Add("philip");
            HeadNames.Add("ryan");
            HeadNames.Add("sharvil");
            HeadNames.Add("tim");

            foreach (string name in HeadNames)
            {
                Graphics.Current.SpritesByName.Add(name, Content.Load<Texture2D>($"Sprites/{name}"));
            }


            Graphics.Current.Fonts = new Dictionary<string, SpriteFont>();
            Graphics.Current.Fonts.Add("arial-48", Content.Load<SpriteFont>($"Fonts/arial_48"));
            Graphics.Current.Fonts.Add("arial-72", Content.Load<SpriteFont>($"Fonts/arial_72"));
            Graphics.Current.Fonts.Add("arial-96", Content.Load<SpriteFont>($"Fonts/arial_96"));
            Graphics.Current.Fonts.Add("arial-144", Content.Load<SpriteFont>($"Fonts/arial_144"));

            circleCropEffect = Content.Load<Effect>("Shaders/circlecrop");
        }
        protected override void Update(GameTime gameTime)
        {
            CheckInput(); // Used to exit game when input detected (aka screensaver logic)
            if (RunSetup) { Setup(); }

            foreach (Head head in Heads)
            {
                head.Update();
            }
            CleanUpDeadHeads();

            GameTurret.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            // Color that the screen is wiped with each frame before drawing anything else:
            Graphics.Current.Device.Clear(Color.Black);

            DrawBackground();
            DrawHeadBoundaries();
            DrawHeads();
            GameTurret.Draw();

            base.Draw(gameTime);
        }

        protected void Setup()
        {
            List<string> namelist = new List<string>(HeadNames);
            for (int i = 0; i < 16; i++)
            {
                int size = Ran.Current.Next(90, 140);
                int index = Ran.Current.Next(0, namelist.Count - 1);
                Heads.Add(new Head(namelist[index], size));
                namelist.RemoveAt(index);
            }

            foreach (Head head in Heads)
            {
                head.SetPosition(
                    Ran.Current.Next(100, Graphics.Current.ScreenWidth - 100),
                    Ran.Current.Next(100, 400));
            }

            GameTurret = new Turret();

            RunSetup = false;
        }

        protected void DrawBackground()
        {
            // Draw your background image here if you want one:

            //Rectangle destinationRectangle = new Rectangle(0, 0, Graphics.Current.ScreenWidth, Graphics.Current.ScreenHeight);
            //Graphics.Current.SpriteB.Begin();
            //Graphics.Current.SpriteB.Draw(
            //    Graphics.Current.SpritesByName["sprite_name"],  // Sprite: (texture2d)
            //    destinationRectangle,
            //    Color.White
            //);
            //Graphics.Current.SpriteB.End();
        }
        protected void CheckInput()
        {
            loadFrames++;
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            if (loadFrames >= LOAD_FRAMES_THRESH)
            {
                // Check if any key was pressed
                if (_currentKeyboardState.GetPressedKeys().Length > 0 && _previousKeyboardState.GetPressedKeys().Length == 0)
                {
                    Exit();
                }

                // Check if the mouse has moved
                Vector2 currentPos = new Vector2(_currentMouseState.Position.X, _currentMouseState.Position.Y);
                Vector2 previousPos = new Vector2(_previousMouseState.Position.X, _previousMouseState.Position.Y);
                if (Global.ApproxDist(currentPos, previousPos) >= 1)
                {
                    Exit();
                }
            }
        }
        protected void DrawHeads()
        {
            Graphics.Current.SpriteB.Begin(effect: circleCropEffect);
            for (int i = 0; i < Heads.Count; i++)
            {
                int spritesize = Heads[i].Radius*2;
                Rectangle rect = new Rectangle(Heads[i].X - Heads[i].Radius, Heads[i].Y - Heads[i].Radius, spritesize, spritesize);
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName[Heads[i].SpriteName], rect, null, Heads[i].GetHighlightColor());

                //Heads[i].Collider.Draw(Color.Red);
            }
            Graphics.Current.SpriteB.End();
        }
        public void DrawHeadBoundaries()
        {
            Graphics.Current.SpriteB.Begin();
            Rectangle rect = Graphics.Current.GetBoundaryRect();
            Graphics.Current.SpriteB.DrawRectangle(rect, Color.Red);
            Graphics.Current.SpriteB.End();
        }
        public void CleanUpDeadHeads()
        {
            List<Head> deadheads = new List<Head>();
            foreach (Head head in Heads)
            {
                if (head.IsDead)
                {
                    deadheads.Add(head);
                }
            }
            foreach (Head head in deadheads)
            {
                Heads.Remove(head);
            }
        }
    }
}