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
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public float Radius { get; set; }
        public CircleCollision(int centerX, int centerY, float radius)
        {
            CenterX = centerX;
            CenterY = centerY;
            Radius = radius;
        }
        public bool IsCollision(int x, int y)
        {
            if (WithinRadius(Radius, x, y, CenterX, CenterY)) { return true; }
            else return false;
        }
        public bool IsCollision(CircleCollision c)
        {
            float largestCircle = Math.Max(Radius, c.Radius);
            return Global.ApproxDist(this.CenterX, this.CenterY, c.CenterX, c.CenterY) <= largestCircle;
        }
        public bool IsCollision(RectangleCollision r)
        {
            return r.IsCollision(this);
        }
        public void Draw(Color? drawColor)
        {
            Graphics.Current.SpriteB.DrawCircle((float)this.CenterX - Radius, (float)this.CenterY - Radius, (float)this.Radius, 16, drawColor == null ? Color.Red : (Color)drawColor);
        }

        public void UpdatePosition(int x, int y)
        {
            this.CenterX = x;
            this.CenterY = y;
        }

        #region Internal
        private bool WithinRadius(float radius, int x1, int y1, int x2, int y2)
        {
            if (radius < 1) { throw new Exception("Bro, seriously?"); }
            int approxDist = Global.ApproxDist(x1, y1, x2, y2);
            if (radius >= approxDist) { return true; }
            else return false;
        }
        #endregion
    }
}
