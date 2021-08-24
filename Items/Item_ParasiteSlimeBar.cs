using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Rarities;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Items
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 5:25:13")]
    public class Item_ParasiteSlimeBar : AscensionItem
    {
        protected override string TooltipDefault { get; } = "This is...I think asking questions here is a bad idea.";

        protected override int JourneyCheatCount { get; } = 10;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(silver: 10);

            Item.rare = ModContent.RarityType<Rarity_Common>();
        }
    }
}
