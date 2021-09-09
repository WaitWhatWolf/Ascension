using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Tiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Placeables
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/09 20:24:20")]
    public sealed class Item_ParasiteSlimeBlock : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Hmm...Maybe I shouldn't hold it with my bare hands?";
        protected override int JourneyCheatCount { get; } = 500;

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tile_ParasiteSlime>();
        }

        protected override string TextureSubFolder => ASCResources.ASSETS_SUBPATH_PLACEABLES;
    }
}
