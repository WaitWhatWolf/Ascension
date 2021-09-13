using Ascension.Attributes;
using Ascension.Biomes;
using Ascension.Enums;
using Ascension.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Internal
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/12 19:04:52")]
    internal sealed class AscensionModSystem : ModSystem
    {
        public readonly Dictionary<EAscensionBiome, int> BiomeTiles = new();

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            if (!BiomeTiles.TryAdd(EAscensionBiome.SlimeChasm, tileCounts[ModContent.TileType<Tile_ParasiteSlime>()]))
            {
                BiomeTiles[EAscensionBiome.SlimeChasm] = tileCounts[ModContent.TileType<Tile_ParasiteSlime>()];
            }
        }
    }
}
