using System;
using MonogameLearning;

namespace Arrrive_Pursue_Behaviour
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new ArrivePursueDemo())
                game.Run();
        }
    }
#endif
}
