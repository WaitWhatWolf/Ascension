using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Armors
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Head)]
    class SamuraiHelmet : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Spooky yet cool."
                + "\n+2% melee crit chance"
                + "\n+10% melee speed attack"
                + "\n+20 max mana";

        protected override int JourneyCheatCount { get; } = 1;

        public override void SetDefaults()
        {
            Item.Size = new Vector2(18);
            Item.value = Item.sellPrice(silver: 24);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 20;
            player.GetCritChance<MeleeDamageClass>() += 2;
            player.meleeSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(TileID.WorkBenches)
            .AddTile(TileID.Anvils)
            .AddIngredient(ModContent.ItemType<LeatherPlate>(), 15)
            .Register();
        }

    }
}
