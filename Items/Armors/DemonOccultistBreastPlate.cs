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
using Ascension.Tiles;
using Ascension.Items;
using Ascension.Enums;
using Ascension.Attributes;

namespace Ascension.Items.Armors
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Body)]
    class DemonOccultistBreastPlate : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Breastplate");
            Tooltip.SetDefault("Extremely heavy and full of" 
                +"\nsquishy material."
                + "\n-10% melee crit chance"
                + "\n-10% melee speed attack");
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(18);
            Item.value = Item.sellPrice(silver: 24);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 9;
        }
        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<MeleeDamageClass>() -= 10;
            player.meleeSpeed -= 0.10f;
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
