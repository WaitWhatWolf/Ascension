using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Ascension.Items;

namespace Ascension.Armors
{
    [AutoloadEquip(EquipType.Head)]
    class SamuraiHelmet : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Samurai Helmet");
            Tooltip.SetDefault("Spooky yet cool."
                + "\n+2% melee crit chance"
                + "\n+10% melee speed attack"
                + "\n+20 max mana");
        }

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
