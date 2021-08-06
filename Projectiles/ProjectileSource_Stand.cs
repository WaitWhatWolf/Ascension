using Ascension.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace Ascension.Projectiles
{
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
