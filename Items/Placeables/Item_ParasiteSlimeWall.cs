using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Walls;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Placeables
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/10 11:08:19")]
    public sealed class Item_ParasiteSlimeWall : AscensionItem
    {
        protected override string TooltipDefault { get; } = ASCResources.Trademark.PARASITESLIME_PLACEABLE_TOOLTIP;
        protected override string TextureSubFolder { get; } = ASCResources.ASSETS_SUBPATH_PLACEABLES;
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
            Item.createWall = ModContent.WallType<Wall_ParasiteSlime>();
        }
    }
}
