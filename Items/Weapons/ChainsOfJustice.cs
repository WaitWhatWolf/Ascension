using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Items.Projectiles;
using Ascension.Items.Rarities;
using Ascension.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Items.Weapons
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.Adragon, "9/13/2021 4:49:57 PM")]
    public class ChainsOfJustice : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }
        protected override string TooltipDefault { get; } = "Does not shake upon the touch of the owner.";
        protected override int JourneyCheatCount { get; } = 2;
        protected override string TextureSubFolder { get; } = ASCResources.ASSETS_SUBPATH_WEAPONS;

        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.noMelee = false;
            Item.DamageType = ModContent.GetInstance<UmbralDamageClass>();
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = false; //so the Item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = 10000;
            Item.rare = ModContent.RarityType<Rarity_VeryRare>();
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<ChainsOfJusticeProjectile>();
            Item.shootSpeed = 12f;
        }


        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item1, player.position);
            Projectile.NewProjectile(source, player.position.X + player.width / 2, player.position.Y + player.height / 2, velocity.X, velocity.Y, ModContent.ProjectileType<ChainsOfJusticeProjectile>(), Item.damage, (int)knockback, Main.myPlayer);
            return false;
        }
    }
}
