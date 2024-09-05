using HPScreen;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using Omnipop.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Entities
{
    public class Shot
    {
        public const float MAX_VELOCITY = 10;
        public const int RADIUS = 16;
        public int X { get; set; }
        public int Y { get; set; }
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public Vector2 CenterPos { get { return new Vector2(X, Y); } }
        public CircleCollision Collider { get; set; }
        public bool IsDead { get; set; }

        public Shot()
        {
            X = 0;
            Y = 0;
            Collider = new CircleCollision(X, Y, RADIUS);
            XVelocity = 0;
            YVelocity = 0;
            IsDead = false;
        }
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            Collider.UpdatePosition(x, y);
        }
        public void AddSpeedX(float speed)
        {
            XVelocity += (int)speed;
            XVelocity = Math.Clamp(XVelocity, -MAX_VELOCITY, MAX_VELOCITY);
        }
        public void AddSpeedY(float speed)
        {
            YVelocity += (int)speed;
            YVelocity = Math.Clamp(YVelocity, -MAX_VELOCITY, MAX_VELOCITY);
        }

        public void Update()
        {
            X += (int)XVelocity;
            Y += (int)YVelocity;
            Collider.UpdatePosition(X, Y);

            CalculateEntityBoundaries();
            CheckHeadCollisions();
        }

        public void Draw()
        {
            Vector2 drawpos = new Vector2(X - RADIUS, Y - RADIUS);

            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["ball"], drawpos, null, Color.White, 0, Vector2.Zero, 1.0f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

            Collider.Draw(Color.Red);

            Graphics.Current.SpriteB.End();

            
        }
        protected void CalculateEntityBoundaries()
        {
            if (Graphics.Current.PositionOutsideBoundary(X, Y))
            {
                if (X < Graphics.Current.GetBoundaryLeft() || X > Graphics.Current.GetBoundaryRight())
                {
                    XVelocity = -XVelocity;
                }

                if (Y < -100 || Y > Graphics.Current.ScreenHeight + 100)
                {
                    IsDead = true;
                }
            }
        }
        protected void CheckHeadCollisions()
        {
            foreach (Head h in ScreenSaver.Heads)
            {
                if (Collider.IsCollision(h.Collider))
                {
                    h.IsDead = true;
                    //IsDead = true;
                }
            }
        }
    }
}
