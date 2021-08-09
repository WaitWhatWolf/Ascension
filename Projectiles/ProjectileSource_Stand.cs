using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Terraria.DataStructures;

namespace Ascension.Projectiles
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class ProjectileSource_Stand : IProjectileSource
    {
        public ProjectileSource_Stand(AscendedPlayer player, Stand stand)
        {
            Stand = stand;
            Player = player;
        }

        public readonly AscendedPlayer Player;
        public readonly Stand Stand;
    }
}
