using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Placeable
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class VoidBlock : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Block");
            Tooltip.SetDefault("This item will create a tile.");
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.createTile = ModContent.TileType<VoidTile>();
            //Item.pick = 150;
        }
        
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(ModContent.TileType<VoidAltar>())
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 1)
            .Register();
        }
        
    }
}
