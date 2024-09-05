using HPScreen;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Entities
{
    public class Turret
    {
        const int SPRITE_WIDTH = 128;
        const int SPRITE_HEIGHT = 128;
        public List<Shot> Shots = new List<Shot>();
        public Turret()
        {
            Rotation = 0;
            TargetRotation = 0;
            TimerMin = 40;
            TimerMax = 180;
            ResetTimer();
            ResetRotation();
            X = Graphics.Current.ScreenMidX;
            Y = Graphics.Current.ScreenHeight;
        }

        public void Fire()
        {
            // Turret information
            float barrelLength = 64f;

            float centerx = (X - SPRITE_WIDTH/2);
            float centery = (Y - SPRITE_HEIGHT/2);

            // Calculate the offset of the bullet's starting position
            float bulletX = centerx + (float)Math.Cos(Rotation) * barrelLength;
            float bulletY = centery + (float)Math.Sin(Rotation) * barrelLength;

            Shot shot = new Shot();
            shot.SetPosition((int)bulletX, (int)bulletY);
            
            shot.AddSpeedX((float)Math.Cos(Rotation) * Shot.MAX_VELOCITY);
            shot.AddSpeedY((float)Math.Sin(Rotation) * Shot.MAX_VELOCITY);
            Shots.Add(shot);
        }
        public int Timer { get; set; }
        public int TimerMin { get; set; }
        public int TimerMax { get; set; }
        public float Rotation { get; set; }
        public float TargetRotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void ResetTimer()
        {
            Timer = new Random().Next(TimerMin, TimerMax);
        }
        public void ResetRotation()
        {
            float angleDegrees = Ran.Current.Next(-160, -20);
            TargetRotation = MathHelper.ToRadians(angleDegrees); // Convert to radians;
        }
        public void Update()
        {
            Rotation = Global.Ease(Rotation, TargetRotation, 0.05f);

            Timer--;
            if (Timer <= 0)
            {
                ResetTimer();
                Fire();
                ResetRotation();
            }

            foreach (Shot shot in Shots)
            {
                shot.Update();
            }
            CleanUpDeadShots();
        }
        public void Draw()
        {
            Vector2 drawpos = new Vector2(X - SPRITE_WIDTH / 2, Y - SPRITE_HEIGHT / 2);
            Vector2 draworigin = new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2);

            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["turretbarrel"], drawpos, null, Color.White, Rotation, draworigin, 1.0f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["turret"], drawpos, null, Color.White, 0, draworigin, 1.0f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            Graphics.Current.SpriteB.End();

            foreach (Shot shot in Shots)
            {
                shot.Draw();
            }
        }

        public void CleanUpDeadShots()
        {
            List<Shot> deadshots = new List<Shot>();
            foreach (Shot shot in Shots)
            {
                if (shot.IsDead)
                {
                    deadshots.Add(shot);
                }
            }
            foreach (Shot shot in deadshots)
            {
                Shots.Remove(shot);
            }
        }
    }
}
