using Ascension.Attributes;
using Ascension.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources.Items;
using Ascension.Projectiles;

namespace Ascension.Items.Consumables
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/31 6:12:56")]
    public class Item_ParasiteSlimeArrow : AscensionItem
    {
        protected override string TooltipDefault { get; } = "Arrows explode on impact;" + TOOLTIP_PARASITESLIMEEXPLOSION;

        protected override int JourneyCheatCount { get; } = 999;

        protected override string TextureSubFolder { get; } = ASCResources.ASSETS_SUBPATH_CONSUMABLES;

        public override void SetDefaults()
        {
            Item.width = 13;
            Item.height = 32;
            Item.DamageType = DamageClass.Ranged;
            Item.maxStack = 1337;
            Item.damage = 1;
            Item.consumable = true;
            //Item.notAmmo = false;
            Item.shoot = ModContent.ProjectileType<Projectile_ParasiteSlimeArrow>();
            Item.shootSpeed = 6f;
            Item.ammo = AmmoID.Arrow;
        }
    }
}
