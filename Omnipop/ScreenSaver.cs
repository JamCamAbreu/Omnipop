﻿using HPScreen.Admin;
using HPScreen.Entities;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        protected List<string> headimages = new List<string>();

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

            // Add your Sprites here like the following:
            Graphics.Current.SpritesByName.Add("cam", Content.Load<Texture2D>("Sprites/cam"));
            Graphics.Current.SpritesByName.Add("jacob", Content.Load<Texture2D>("Sprites/jacob"));

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

            // Add your entity update logic here:
            //your_entity.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            // Color that the screen is wiped with each frame before drawing anything else:
            Graphics.Current.Device.Clear(Color.Black);

            DrawBackground();
            DrawHeads();

            base.Draw(gameTime);
        }

        protected void Setup()
        {
            headimages.Add("cam");
            headimages.Add("jacob");
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
            int size = 140;
            for (int i = 0; i < headimages.Count; i++)
            {
                Rectangle rect = new Rectangle((int)(i * (size*1.5f)), 0, size, size);
                Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName[headimages[i]], rect, null, Color.White);
            }
            Graphics.Current.SpriteB.End();
        }
    }
}