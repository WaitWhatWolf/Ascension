using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Ascension.Items.Rarities;

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
            DisplayName.SetDefault("Stand Arrow");
            Tooltip.SetDefault("Has the power to manifest someone's will.");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.maxStack = 1;
            Item.value = 0;
            Item.useStyle = 0;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = false;

            Item.rare = ModContent.RarityType<Rarity_Chromic>();
            Item.UseSound = SoundID.Item115;
        }

        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            Debug.Log("Your will has manifested! But it's not implemented yet so fuck off");
            player.Hurt(ASCResources.DeathReasons.GetReason("STANDARROW", player.name), player.statLifeMax - 1, -1);
            if(!player.dead)
            {
                //Do the arrow thing
            }

            return true;
        }
    }
}
