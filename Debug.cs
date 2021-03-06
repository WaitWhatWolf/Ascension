using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using static Ascension.ASCResources;

namespace Ascension
{
    /// <summary>
    /// A debug class used to output info.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
    public static class Debug
    {
        public static void Chat(object message, Color color)
        {
            Main.NewTextMultiline(message.ToString(), c: color);
        }

        public static void Log(object message)
        {
            Main.NewTextMultiline(message?.ToString() ?? STRING_NULL, c: Color.LightCyan);
        }

        public static void LogWarning(object message)
        {
            Main.NewTextMultiline(message?.ToString() ?? STRING_NULL, c: Color.DarkOrange);
        }

        public static void LogError(object message)
        {
            Main.NewTextMultiline(message?.ToString() ?? STRING_NULL, c: Color.Crimson);
        }

        public static void LogEnumerable<T>(IEnumerable<T> list)
        {
            foreach(T item in list)
            {
                Log(item);
            }
        }
    }
}
