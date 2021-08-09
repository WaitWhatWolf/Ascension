using Ascension.Attributes;
using Ascension.Enums;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Placeable
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class VoidAltarP : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Void Altar used for sacrifices to the void");
        }


        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 28;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = 150;
            Item.createTile = ModContent.TileType<Tiles.VoidAltar>();
            //Item.pick = 150;
            
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(ItemID.Hellforge)
            .AddIngredient(ModContent.ItemType<PortalCatalyst>(), 10)
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 100)
            .Register();
        }
    }
}
