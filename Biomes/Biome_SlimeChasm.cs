using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Biomes
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/12 17:13:52")]
    public sealed class Biome_SlimeChasm : ModBiome
    {
        public override bool IsPrimaryBiome => true;
        public override Color? BackgroundColor => ASCResources.Trademark.Biome_SlimeChasm_Theme_Color;
        public override ModWaterStyle WaterStyle => ModContent.GetInstance<WaterStyle_SlimeChasmLayer1>();
        public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<SBGStyle_SlimeChasm>();
        public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Mushroom;

        public override bool IsBiomeActive(Player player)
        {
            return ModContent.GetInstance<AscensionModSystem>().BiomeTiles[EAscensionBiome.SlimeChasm] >= 80;

            return false;
        }
    }
}
