using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Ascension.Items.Rarities;
using Ascension.Players;
using static Ascension.ASCResources;
using Ascension.Enums;
using Ascension.Attributes;

namespace Ascension.Items
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 04)]
    public sealed class Item_StandArrow : AscensionItem
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
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.useTurn = false;
            Item.UseSound = SoundID.Item115;

            Item.maxStack = 1;
            Item.value = 0;
            Item.autoReuse = false;

            Item.rare = ModContent.RarityType<Rarity_Chromic>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.whoAmI == Main.myPlayer;
        }

        public override bool? UseItem(Player player)
        {
            player.Hurt(ASCResources.DeathReasons.GetReason("STANDARROW", player.name), player.statLifeMax - 1, -1);
            if(!player.dead)
            {
                AscendedPlayer modPlayer = player.GetModPlayer<AscendedPlayer>();
                
                if(!modPlayer.in_IsStandUser)
                    ASCResources.Players.ManifestStand(modPlayer);

                modPlayer.in_Stand.TryUnlockAbilities();
            }

            return true;
        }
    }
}
