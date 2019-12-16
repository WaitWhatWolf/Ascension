using System.Collections.Generic;
using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.UI
{
    /// <summary>
    /// Core class for all UI of the WarWolfWorks mod.
    /// </summary>
    public abstract class Menu : Terraria.UI.UIElement
    {
        /// <summary>
        /// All menus currently initiated.
        /// </summary>
        internal static List<Menu> AllMenus { get; } = new List<Menu>();
        /// <summary>
        /// Menus to be initiated on mod start.
        /// </summary>
        internal static IEnumerable<Menu> InitiationMenus { get; } = new Menu[]
        {

        };

        /// <summary>
        /// Called right after the world is loaded through <see cref="WarWolfWorks_Mod.Internal.WWWPlayer"/>.
        /// </summary>
        public virtual void OnWorldLoaded(WWWPlayer from) { }
    }
}
