using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Armors
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08), AutoloadEquip(EquipType.Legs)]
    class DemonOccultistBoots : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Boots");
            Tooltip.SetDefault("Heavy and slippery."
                + "\n-20% movement speed");
        }

        public override void SetDefaults()
        {
            Item.Size = new Vector2(18);
            Item.value = Item.sellPrice(silver: 24);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed = player.moveSpeed - 0.2f;
        }

        public override bool DrawLegs()
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
