using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Items.Rarities;
using Ascension.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources.Items;

namespace Ascension.Items.Weapons
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10)]
    public sealed class Item_ParasiteSlimeJavelin : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Throws Slime Javelins which explode on impact." + TOOLTIP_PARASITESLIMEEXPLOSION;
        protected override int JourneyCheatCount { get; } = 1;
        protected override string TextureSubFolder { get; } = ASCResources.ASSETS_SUBPATH_WEAPONS;

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Javelin);

            Item.height = 35;
            Item.width = 35;
            Item.DamageType = ModContent.GetInstance<UmbralDamageClass>();
            Item.damage = 3;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.shoot = ModContent.ProjectileType<Projectile_ParasiteSlimeJavelin>();

            Item.rare = ModContent.RarityType<Rarity_Common>();
        }
    }
}
