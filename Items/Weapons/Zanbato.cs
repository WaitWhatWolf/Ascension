using Microsoft.Xna.Framework;
using Ascension.Items.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Ascension.Enums;
using Ascension.Attributes;

namespace Ascension.Items.Weapons
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class Zanbato : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Feels very light upon pickup");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, "Weapons/", this);

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Throwing;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true; //so the Item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = 1000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<PlayerSmashWave>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            MakeDustWeak();
            SoundEngine.PlaySound(SoundID.Item7, player.position);
            return true;
        }


        public void MakeDustWeak()
        {
            int dust = Dust.NewDust(Item.position, Item.width + Main.rand.Next(-50, 51), Item.height + Main.rand.Next(-50, 51), DustID.WhiteTorch, 0f, 0f, 50, default(Color), 1f); //56 blue or 54 black
            Main.dust[dust].velocity *= 1.2f;
            Main.dust[dust].noGravity = true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            MakeDustWeak();
        }
    }
}
