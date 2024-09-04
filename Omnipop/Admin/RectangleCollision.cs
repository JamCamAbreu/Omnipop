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
    public class RectangleCollision : ICollision
    {
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int LeftX { get { return CenterX - Width / 2; } }
        public int RightX { get { return CenterX + Width / 2; } }
        public int TopY { get { return CenterY - Height / 2; } }
        public int BottomY { get { return CenterY + Height / 2; } }
        public RectangleCollision(int centerX, int centerY, int width, int height)
        {
            CenterX = centerX;
            CenterY = centerY;
            Width = width;
            Height = height;
        }
        public bool IsCollision(int x, int y)
        {
            if (x >= CenterX - Width / 2 && x <= CenterX + Width / 2 &&
                y >= CenterY - Height / 2 && y <= CenterY + Height / 2)
            {
                return true;
            }
            return false;
        }
        public bool IsCollision(RectangleCollision collider)
        {
            bool overlapX = (LeftX < (collider.LeftX + collider.Width)) &&
                            ((LeftX + Width) > collider.LeftX);
            bool overlapY = (TopY < (collider.TopY + collider.Height)) &&
                            ((TopY + Height) > collider.TopY);
            return overlapX && overlapY;
        }
        public bool IsCollision(CircleCollision collider)
        {
            // Find the closest point on the rectangle to the center of the circle
            float closestX = Clamp(collider.CenterX, this.CenterX - this.Width / 2, this.CenterX + this.Width / 2);
            float closestY = Clamp(collider.CenterY, this.CenterY - this.Height / 2, this.CenterY + this.Height / 2);

            // Calculate the distance between the collider's center and the closest point on the rectangle
            float distanceX = collider.CenterX - closestX;
            float distanceY = collider.CenterY - closestY;

            // Check if the distance is less than or equal to the collider's radius
            return (distanceX * distanceX + distanceY * distanceY) <= (collider.Radius * collider.Radius);

        }
        public void Draw(Color? drawColor = null)
        {
            Graphics.Current.SpriteB.DrawRectangle((float)this.CenterX - this.Width / 2, (float)this.CenterY - this.Height / 2, (float)this.Width, (float)this.Height, drawColor == null ? Color.Red : (Color)drawColor);
        }

        public void UpdatePosition(int x, int y)
        {
            this.CenterX = x;
            this.CenterY = y;
        }

        static float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}
