using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnipop.Admin
{
    public interface ICollision
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsCollision(int x, int y);
        public void UpdatePosition(int x, int y);
        public void Draw(Color? drawColor);
    }
}
