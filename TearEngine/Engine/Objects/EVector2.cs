using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TearEngine.Engine.Objects
{
    /// <summary>
    /// Euclidean vector
    /// </summary>
    class EVector2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public EVector2(int X = 0, int Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }

        public static EVector2 operator +(EVector2 a, EVector2 b) => new EVector2(a.X + b.X, a.Y + b.Y);

        public static EVector2 operator +(EVector2 a, int b) => new EVector2(a.X + b, a.Y + b);

        public static EVector2 operator -(EVector2 a, EVector2 b) => new EVector2(a.X - b.X, a.Y - b.Y);
    }
}
