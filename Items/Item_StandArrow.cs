using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Ascension.Items
{
    public sealed class Item_StandArrow : ModItem
    {
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddCondition(NetworkText.FromKey("RecipeConditions.LowHealth"), r => Main.LocalPlayer.statLife < Main.LocalPlayer.statLifeMax / 10)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Has the power to manifest someone's will.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.maxStack = 1;
            Item.value = 0;
        }

        
        public override bool? UseItem(Player player)
        {
            player.Hurt(ASCResources.DeathReasons.GetReason("STANDARROW", player), player.statLifeMax - 1, -1);
            if(!player.dead)
            {
                //Do the arrow thing
            }

            return null; //This is here just so the project compiles, will remove it later
        }
    }
}
