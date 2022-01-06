using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TearEngine.Games
{
    internal class Maps
    {
        public static string[,] EmptyChunk = new string[,]
        {
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
        };

        /// <summary>
        /// Flat chunk
        /// </summary>
        public static string[,] Chunk1 = new string[,]
        {
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp" },
            { "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
            { "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
            { "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
        };

        public static string[,] Chunk2 = new string[,]
        {
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
            { "GroundUp", "GroundUp", "GroundRightEdgeTop", "", "", "", "GroundLeftEdgeTop", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp", "GroundUp" },
            { "GroundNone", "GroundNone", "GroundRight", "", "", "", "GroundLeft", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
            { "GroundNone", "GroundNone", "GroundRight", "", "", "", "GroundLeft", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
            { "GroundNone", "GroundNone", "GroundRight", "", "", "", "GroundLeft", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone", "GroundNone" },
        };
    }
}
