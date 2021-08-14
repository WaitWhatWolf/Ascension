using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Ascension.Tiles;
using Ascension.Items;
using Ascension.Attributes;
using Ascension.Enums;

namespace Ascension.Items.Armors
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Head)]
    class DemonOccultistHelmet : AscensionItem
    {
        protected override string DisplayNameDefault { get; } = "Void Helmet";
        protected override string TooltipDefault { get; } = "Heavy and full of squishy material."
                + "\n-20% melee crit chance"
                + "\n-5% melee speed attack";
        protected override int JourneyCheatCount { get; } = 1;

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
