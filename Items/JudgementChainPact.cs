using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Rarities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.Adragon, "9/10/2021 6:55:37 PM")]
    public class JudgementChainPact : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Pact given by the prophet";
        protected override int JourneyCheatCount { get; } = 3;

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<Rarity_Uncommon>();
            Item.value = Item.sellPrice(silver: 1);
            Item.maxStack = 999;
        }
    }
}
