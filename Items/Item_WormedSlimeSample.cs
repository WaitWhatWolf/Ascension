using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Rarities;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Ascension.Items
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class Item_WormedSlimeSample : AscensionItem
    {
        protected override string TooltipDefault { get; } = "King Slime's Gel.\nLooks like something lives inside...";
        protected override int JourneyCheatCount { get; } = 20;

        public override void SetDefaults()
        {
            Item.height = 32;
            Item.width = 32;
            Item.maxStack = 99;

            Item.rare = ModContent.RarityType<Rarity_Common>();
        }
    }
}
