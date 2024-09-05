using HPScreen;
using HPScreen.Admin;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using Omnipop.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Entities
{
    public class Head
    {
        public const float ACCELERATION = 0.15f;
        public const float MAX_VELOCITY = 5;
        public const float AIR_FRICTION = 0.996f;
        public string SpriteName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        public Vector2 CenterPos { get { return new Vector2(X, Y); } }
        public int Radius { get; protected set; }
        public CircleCollision Collider { get; set; }
        public bool IsDead { get; set; }
        public Head(string spriteName, int radius)
        {
            SpriteName = spriteName;
            Radius = radius;
            X = 0;
            Y = 0;
            Collider = new CircleCollision(X, Y, radius);
            IsDead = false;
        }
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
            Collider.UpdatePosition(x, y);
        }
        public void Update()
        {
            CalculateEntityBoundaries(ScreenSaver.Heads);
            ApplyAirFriction();

            X += (int)XVelocity;
            Y += (int)YVelocity;
            Collider.UpdatePosition(X, Y);
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
        public void SetRadius(int radius)
        {
            Radius = radius;
            Collider.Radius = radius;
        }
        protected void CalculateEntityBoundaries(IEnumerable<Head> comparedto)
        {
            foreach (Head e in comparedto)
            {
                if (e == this)
                {
                    continue;
                }
                float centerdist = Global.ApproxDist(CenterPos, e.CenterPos);
                float overlap = centerdist - (Radius + e.Radius);
                if (centerdist < this.Radius + e.Radius)
                {
                    Vector2 pushvect = Global.GetVectorBetweenTwoPoints(X, Y, e.X, e.Y);
                    pushvect.Normalize();
                    pushvect *= Math.Abs(overlap) * ACCELERATION;

                    AddSpeedX(pushvect.X);
                    AddSpeedY(pushvect.Y);

                    e.AddSpeedX(-pushvect.X);
                    e.AddSpeedY(-pushvect.Y);
                }
            }

            if (Graphics.Current.PositionOutsideBoundary(X, Y))
            {
                if (X < Graphics.Current.GetBoundaryLeft() || X > Graphics.Current.GetBoundaryRight())
                {
                    XVelocity = -XVelocity;
                }
                if (Y < Graphics.Current.GetBoundaryTop() || Y > Graphics.Current.GetBoundaryBottom())
                {
                    YVelocity = -YVelocity;
                }
            }
        }
        protected void ApplyAirFriction()
        {
            XVelocity *= AIR_FRICTION;
            YVelocity *= AIR_FRICTION;
        }

    }
}
