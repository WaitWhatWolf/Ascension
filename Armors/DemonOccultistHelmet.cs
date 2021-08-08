﻿using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Ascension.Tiles;
using Ascension.Items;

namespace Ascension.Armors
{
    [AutoloadEquip(EquipType.Head)]
    class DemonOccultistHelmet : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Helmet");
            Tooltip.SetDefault("Heavy and full of squishy material."
                + "\n-20% melee crit chance"
                + "\n-5% melee speed attack");        
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(24);
            Item.value = Item.sellPrice(silver: 24);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DemonOccultistBreastPlate>() && legs.type == ModContent.ItemType<DemonOccultistBoots>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Life regeneration doubled but lowers acceleration";
            player.lifeRegen *= 2;
            player.lifeRegenTime /= 2;
            //player.gravControl = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<MeleeDamageClass>() -= 20;
            player.meleeSpeed -= 0.05f;
        }

        public override bool DrawHead()
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(ModContent.TileType<VoidAltar>())
            .AddIngredient(ModContent.ItemType<PortalCatalyst>(), 1)
            .AddIngredient(ModContent.ItemType<VoidFragment>(), 10)
            .Register();
        }
    }
}
