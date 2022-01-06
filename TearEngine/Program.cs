using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TearEngine
{
    static class Program
    {
        static void Main()
        {
            Games.Game game = new Games.Game();
            game.Run();
        }
    }
}
