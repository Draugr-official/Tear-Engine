
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TearEngine.Engine.Objects
{
    /// <summary>
    /// 4 point
    /// </summary>
    class Vertex4
    {
        public EVector2 TopLeft = new EVector2();

        public EVector2 TopRight = new EVector2();

        public EVector2 BottomRight = new EVector2();

        public EVector2 BottomLeft = new EVector2();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="TopLeft"></param>
        /// <param name="TopRight"></param>
        /// <param name="BottomLeft"></param>
        /// <param name="BottomRight"></param>
        public Vertex4(EVector2 TopLeft, EVector2 TopRight, EVector2 BottomLeft, EVector2 BottomRight)
        {
            this.TopLeft = TopLeft;
            this.TopRight = TopRight;
            this.BottomRight = BottomRight;
            this.BottomLeft = BottomLeft;
        }

        public Vertex4()
        {

        }
    }
}
