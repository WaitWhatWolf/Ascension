using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Placeables
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class VoidBlock : AscensionItem
    {
        [Note(Dev.WaitWhatWolf, "Previous tooltip: \"This item will create a tile.\"" +
            "@Adragon if you want to change it back feel free to, I just thought this was probably a forgotten placeholder tooltip.")]
        protected override string TooltipDefault { get; } = "Stare into the void long enough...Oh god it's staring back";
        protected override int JourneyCheatCount { get; } = 100;


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
