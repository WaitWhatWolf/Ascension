using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Armors
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Body)]
    class SamuraiChestplate : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Samurai Helmet");
            Tooltip.SetDefault("Spooky yet cool."
                + "\n+6% melee crit chance"
                + "\n+12% melee speed attack"
                + "\n+20 max mana");
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(18);
            Item.value = Item.sellPrice(silver: 24);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 12;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
            player.GetCritChance<MeleeDamageClass>() += 6;
            player.meleeSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(TileID.WorkBenches)
            .AddTile(TileID.Anvils)
            .AddIngredient(ModContent.ItemType<LeatherPlate>(), 24)
            .Register();
        }

    }
}
