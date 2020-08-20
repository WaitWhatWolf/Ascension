using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WarWolfWorks_Mod.Items
{
    public class LeatherPlate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leather Plate");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(silver: 3);
            item.maxStack = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 200);
            recipe.AddIngredient(ItemID.IronBar, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
