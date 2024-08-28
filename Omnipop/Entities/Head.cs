using HPScreen.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Entities
{
    public class Head
    {
        public string SpriteName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }
        public Head(string spriteName, int radius)
        {
            SpriteName = spriteName;
            Radius = radius;
            X = 0;
            Y = 0;
        }
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
        public bool IsCollision(int x, int y)
        {
            if (Global.ApproxDist(x, y, X, Y) < Radius)
            {
                return true;
            }
            return false;
        }
    }
}
