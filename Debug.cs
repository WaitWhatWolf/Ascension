using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Ascension
{
    /// <summary>
    /// A debug class used to output info.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
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

        public static void LogEnumerable<T>(IEnumerable<T> list)
        {
            foreach(T item in list)
            {
                Log(item);
            }
        }
    }
}
