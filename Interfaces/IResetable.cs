namespace WarWolfWorks_Mod.Interfaces
{
    /// <summary>
    /// Used to reset an object to it's post-constructor state.
    /// </summary>
    public interface IResetable
    {
        /// <summary>
        /// Resets this object to it's post-constructor state.
        /// </summary>
        void Reset();
    }
}
