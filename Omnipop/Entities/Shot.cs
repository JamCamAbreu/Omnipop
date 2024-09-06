using HPScreen;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
        public const float MIN_VELOCITY = 7;
        public const float MAX_VELOCITY = 14;
        public const int RADIUS = 16;
        public int X { get; set; }
        public int Y { get; set; }
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public Vector2 CenterPos { get { return new Vector2(X, Y); } }
        public CircleCollision Collider { get; set; }
        public bool IsDead { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public float TravelingSpeed 
        { 
            get
            {
                return (float)Math.Sqrt(XVelocity * XVelocity + YVelocity * YVelocity);
            } 
        }

        public Shot()
        {
            X = 0;
            Y = 0;
            Collider = new CircleCollision(X, Y, RADIUS);
            XVelocity = 0;
            YVelocity = 0;
            IsDead = false;
            MaxHealth = Ran.Current.Next(5, 60);
            Health = MaxHealth;
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
            if (Health < 0)
            {
                IsDead = true;
            }
        }
        public Color GetHighlightColor()
        {
            return Color.Lerp(Color.White, Color.Red, 1 - (float)Health / MaxHealth);
        }

        public void Draw()
        {
            Vector2 drawpos = new Vector2(X - RADIUS, Y - RADIUS);

            Graphics.Current.SpriteB.Begin();
            Graphics.Current.SpriteB.Draw(Graphics.Current.SpritesByName["ball"], drawpos, null, GetHighlightColor(), 0, Vector2.Zero, 1.0f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);

            //Collider.Draw(Color.Red);

            Graphics.Current.SpriteB.End();

            
        }
        protected void CalculateEntityBoundaries()
        {
            if (Graphics.Current.PositionOutsideBoundary(X, Y))
            {
                if (X < Graphics.Current.GetBoundaryLeft() || X > Graphics.Current.GetBoundaryRight())
                {
                    XVelocity = -XVelocity;
                    Health -= 9;
                }

                if (Y <= 0 || Y >= Graphics.Current.ScreenHeight)
                {
                    YVelocity = -YVelocity;
                    Health -= 9;
                }
            }
        }
        protected void CheckHeadCollisions()
        {
            foreach (Head h in ScreenSaver.Heads)
            {
                if (Collider.IsCollision(h.Collider))
                {
                    // Apply damage
                    int damage = (int)(TravelingSpeed * 2);
                    h.Health -= damage;
                    Health -= damage;

                    // reflect the shot depending on where it hits the head
                    Vector2 curVelocity = new Vector2(XVelocity, YVelocity);
                    Vector2 pushvect = Global.GetVectorBetweenTwoPoints(X, Y, h.X, h.Y);
                    Vector2 normal = pushvect.NormalizedCopy();
                    Vector2 reflected = curVelocity - 2 * Vector2.Dot(curVelocity, normal) * normal;
                    XVelocity = reflected.X;
                    YVelocity = reflected.Y;

                    h.XVelocity = -XVelocity;
                    h.YVelocity = -YVelocity;
                }
            }
        }
    }
}
