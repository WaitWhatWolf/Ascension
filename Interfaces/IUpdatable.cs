namespace WarWolfWorks_Mod.Interfaces
{
    /// <summary>
    /// Used for call-per-frame functionality.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Called every in-game frame.
        /// </summary>
        void Update();
    }
}
