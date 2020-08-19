using WarWolfWorks_Mod.Internal;

namespace WarWolfWorks_Mod.Interfaces
{
    /// <summary>
    /// Used to invoke a method after the player loads into a world.
    /// </summary>
    public interface IPostWorldLoadable
    {
        /// <summary>
        /// Called when the player is first loaded into a world.
        /// </summary>
        /// <param name="for"></param>
        void OnWorldLoaded();
    }
}
