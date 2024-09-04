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
        public static List<Head> headimages = new List<Head>();
        protected List<string> names = new List<string>();
        public List<ICollision> colliders = new List<ICollision>();

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
            names.Add("aaron");
            names.Add("acole");
            names.Add("aj");
            names.Add("alej");
            names.Add("allison");
            names.Add("brett");
            names.Add("brian");
            names.Add("cam");
            names.Add("chad");
            names.Add("chris");
            names.Add("dakota");
            names.Add("dalton");
            names.Add("daryl");
            names.Add("denise");
            names.Add("dustin");
            names.Add("dusty");
            names.Add("emily");
            names.Add("isaac");
            names.Add("jack");
            names.Add("jacob");
            names.Add("jackadam");
            names.Add("jeff");
            names.Add("john");
            names.Add("jonah");
            names.Add("josh");
            names.Add("jpaul");
            names.Add("juan");
            names.Add("katelyn");
            names.Add("keegan");
            names.Add("keiran");
            names.Add("kenny");
            names.Add("kevin");
            names.Add("mary");
            names.Add("max");
            names.Add("michael");
            names.Add("nicole");
            names.Add("noah");
            names.Add("philip");
            names.Add("ryan");
            names.Add("sharvil");
            names.Add("tim");

            foreach (string name in names)
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

            foreach (Head head in headimages)
            {
                head.Update();
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            // Color that the screen is wiped with each frame before drawing anything else:
            Graphics.Current.Device.Clear(Color.Black);

            DrawBackground();
            DrawHeadBoundaries();
            DrawHeads();

            base.Draw(gameTime);
        }

        protected void Setup()
        {
            List<string> namelist = new List<string>(names);
            for (int i = 0; i < 16; i++)
            {
                int size = Ran.Current.Next(90, 140);
                int index = Ran.Current.Next(0, namelist.Count - 1);
                headimages.Add(new Head(namelist[index], size));
                namelist.RemoveAt(index);
            }

            foreach (Head head in headimages)
            {
                head.SetPosition(
                    Ran.Current.Next(100, Graphics.Current.ScreenWidth - 100),
                    Ran.Current.Next(100, 400));
            }

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
            for (int i = 0; i < headimages.Count; i++)
            {
                int spritesize = headimages[i].Radius*2;
                Rectangle rect = new Rectangle(headimages[i].X - headimages[i].Radius, headimages[i].Y - headimages[i].Radius, spritesize, spritesize);
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName[headimages[i].SpriteName], rect, null, Color.White);

                //headimages[i].Collider.Draw(Color.Red);
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
    }
}