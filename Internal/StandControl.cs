using WarWolfWorks_Mod.Interfaces;

namespace WarWolfWorks_Mod.Internal
{
    public abstract class StandControl : IStand
    {
        public Stand StandOwned { get; private set; }

        public StandControl(Stand stand)
        {
            StandOwned = stand;
        }

        public abstract void OnUpdate();
        
    }
}
