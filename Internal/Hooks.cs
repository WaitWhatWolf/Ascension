using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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
                MouseState state = Mouse.GetState();
                return new Vector2(state.X, state.Y);
            }
        }
    }
}
