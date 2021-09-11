using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Items.Rarities;
using Ascension.NPCs;
using Ascension.Tiles;
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
    [CreatedBy(Dev.Adragon, "9/10/2021 7:00:34 PM")]
    public class AzazelPact : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Pact from unknown origin";
        protected override int JourneyCheatCount { get; } = 3;

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.rare = ModContent.RarityType<Rarity_Rare>();
            Item.value = Item.sellPrice(silver: 1);
            Item.maxStack = 999;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 25;
            Item.useTime = 25;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddTile(ModContent.TileType<VoidAltar>())
            .AddIngredient(ModContent.ItemType<ExcaliburPact>(), 1)
            .AddIngredient(ModContent.ItemType<YggdrasilPact>(), 1)
            .AddIngredient(ModContent.ItemType<JudgementChainPact>(), 1)
            .AddIngredient(ModContent.ItemType<KusanagiPact>(), 1)
            .Register();
        }

        public override bool CanUseItem(Player player)
        {
            return player.whoAmI == Main.myPlayer;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<ProphetSwordOccultist>());
            return true;
        }
    }
}
