using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Biomes
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/12 19:19:53")]
    public abstract class AscensionWaterStyle : ModWaterStyle
    {
        public override string Texture => ASCResources.ASSETS_PATH_BIOMES + "WaterStyle_" + Hooks.Text.GetFormatName(this);
        public override string BlockTexture => ASCResources.ASSETS_PATH_BIOMES + "WaterStyleBlock_" + Hooks.Text.GetFormatName(this);
    }
}
