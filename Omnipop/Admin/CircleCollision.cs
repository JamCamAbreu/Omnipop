using HPScreen.Admin;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Admin
{
    public class CircleCollision : ICollision
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Radius { get; set; }
        public CircleCollision(int centerX, int centerY, float radius)
        {
            X = centerX;
            Y = centerY;
            Radius = radius;
        }
        public bool IsCollision(int x, int y)
        {
            float dist = Global.ApproxDist(this.X, this.Y, x, y);
            if (dist <= this.Radius)
            {
                return true;
            }
            return false;
        }
        public bool IsCollision(CircleCollision c)
        {
            float largestCircle = Math.Max(Radius, c.Radius);
            return Global.ApproxDist(this.X, this.Y, c.X, c.Y) <= largestCircle;
        }
        public bool IsCollision(RectangleCollision r)
        {
            return r.IsCollision(this);
        }
        public void Draw(Color? drawColor)
        {
            Graphics.Current.SpriteB.DrawCircle((float)this.X, (float)this.Y, (float)this.Radius, 16, drawColor == null ? Color.Red : (Color)drawColor);
        }

        public void UpdatePosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
