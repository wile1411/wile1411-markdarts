using System;

namespace SuperDarts
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SuperDarts game = new SuperDarts())
            {
                game.Run();
            }
        }
    }
#endif
}

