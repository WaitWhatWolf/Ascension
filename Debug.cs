using Microsoft.Xna.Framework;
using Terraria;

namespace Ascension
{
    /// <summary>
    /// A debug class used to output info.
    /// </summary>
    public static class Debug
    {
        public static void Log(object message)
        {
            Main.NewTextMultiline(message.ToString(), c: Color.LightCyan);
        }

        public static void LogWarning(object message)
        {
            Main.NewTextMultiline(message.ToString(), c: Color.DarkOrange);
        }

        public static void LogError(object message)
        {
            Main.NewTextMultiline(message.ToString(), c: Color.Crimson);
        }
    }
}
