using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Biomes
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/12 17:22:12")]
    public class WaterStyle_SlimeChasmLayer1 : AscensionWaterStyle
    {
        public override Color BiomeHairColor() => ASCResources.Trademark.Biome_SlimeChasm_Theme_Color;
        public override int ChooseWaterfallStyle() => WaterStyleID.Purity;

        public override int GetDropletGore() => GoreID.WaterDrip;

        public override int GetSplashDust() => DustID.t_Slime;
    }
}
