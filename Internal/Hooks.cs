using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// General Utilities for the WarWolfWorks mod.
    /// </summary>
    public static class Hooks
    {
        /// <summary>
        /// Timer used to get a consistent timespan for a counter.
        /// </summary>
        // Takes the current framerate, divides 60 by itself to get a float value which increases the lower the framerate
        // (this is here to get the same speed of adding regardless of framerate)
        // then divides it by 60 assuming it will be called once every frame, therefore 60 times a second.
        public static TimeSpan TimespanCounter => TimeSpan.FromSeconds(Math.Max(1, 60 / Terraria.Main.frameRate) / 60);

        /// <summary>
        /// Timer used to get a consistent timespan for a counter; Divided by 60 for an update-friendly timespan.
        /// </summary>
        public static TimeSpan TimespanCounterUF => TimeSpan.FromSeconds(Math.Max(1, 60 / Terraria.Main.frameRate) / 60);

        /// <summary>
        /// Returns the mouse position in vector value.
        /// </summary>
        public static Vector2 MousePos
        {
            get
            {
                //MouseState state = Mouse.GetState();
                //return new Vector2(state.X, state.Y);
                return new Vector2(Main.mouseX, Main.mouseY);
            }
        }

        /// <summary>
        /// Cuts a float to digits length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static float Truncate(this float value, int digits)
        {
            double num = Math.Pow(10.0, digits);
            double num2 = Math.Truncate(num * value) / num;
            return (float)num2;
        }

        /// <summary>
        /// Cuts a float to digits length.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double Truncate(this double value, int digits)
        {
            double num = Math.Pow(10.0, digits);
            double num2 = Math.Truncate(num * value) / num;
            return num2;
        }

    }
}
