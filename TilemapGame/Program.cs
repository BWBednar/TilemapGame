using System;

namespace TilemapGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TilemapGame())
                game.Run();
        }
    }
}
