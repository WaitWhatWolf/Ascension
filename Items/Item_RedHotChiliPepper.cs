using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using static Ascension.ASCResources.Recipes;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Ascension.Items.Rarities;
using Ascension.Players;
using Ascension.Enums;
using Ascension.Attributes;

namespace Ascension.Items
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
    public sealed class Item_RedHotChiliPepper : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Hot Chilli Pepper");
            Tooltip.SetDefault("Allows the player to resist the arrow another time.\n"
                + Hooks.Colors.GetColoredTooltipText("After consuming the Red Hot Chili Pepper, you can use the", Color.CornflowerBlue)
                + Hooks.Colors.GetColoredTooltipText(" Stand Arrow ", Hooks.Colors.GetColorByRarity(Enums.ERarity.Chromic))
                + Hooks.Colors.GetColoredTooltipText("again to gain new powers.", Color.CornflowerBlue));
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 1);
            Item.useStyle = 0;

            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = false;
            Item.consumable = true;
            Item.UseSound = SoundID.Item2;

            Item.rare = ModContent.RarityType<Rarity_Uncommon>();
        }

        public override bool CanUseItem(Player player) => !player.GetModPlayer<AscendedPlayer>().ConsumedRedHotChiliPepper;
        public override bool? UseItem(Player player)
        {
            Debug.Chat("The arrow is calling your name...", Color.Purple);
            player.GetModPlayer<AscendedPlayer>().ConsumedRedHotChiliPepper = true;

            return true;
        }
    }
}
