using Microsoft.Xna.Framework;
using Ascension.Items.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using Terraria.DataStructures;
using Ascension.Enums;

namespace Ascension.Items.Weapons
{
    class RuneBlade : AscensionItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You make the rules here. Every 5 seconds that is.");
            Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Items, "Weapons/", this);

        public override void SetDefaults()
        {
            Item.damage = 28;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 40;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 300;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false; //so the Item's animation doesn't do damage
            Item.knockBack = 5;
            Item.value = 1000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FallingBlades>();
            Item.shootSpeed = 2f;
        }

        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(SoundID.Item8, Item.position);        
            int num1 = 15;
            for (int index = 0; index < num1; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num2 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num3 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num3 < 0.0) num3 *= -1f;
                if ((double)num3 < 20.0) num3 = 20f;
                float num4 = (float)Math.Sqrt((double)num2 + (double)num3);
                float num5 = Item.shootSpeed;
                float num6 = num2 * num5;
                float num7 = num3 * num5;
                float SpeedX = num6 / 400 + (float)Main.rand.Next(-40, 41) * 0.02f;
                float SpeedY = num7 / 400 + (float)Main.rand.Next(-40, 41) * 0.02f;
                Projectile.NewProjectile(source, vector2_1.X, vector2_1.Y, SpeedX, SpeedY, ModContent.ProjectileType<FallingBlades>(), damage, knockback, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));
            }           
            return true;
        }
    }
}
