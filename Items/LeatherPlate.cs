using Ascension.Attributes;
using Ascension.Enums;
using Terraria;
using Terraria.ID;

namespace Ascension.Items
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    public class LeatherPlate : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leather Plate");
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 3);
            Item.maxStack = 5;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(ItemID.WorkBench)
            .AddIngredient(ItemID.Wood, 200)
            .AddIngredient(ItemID.IronBar,10)
            .Register();
        }
    }
}
