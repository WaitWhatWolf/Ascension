using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Internal;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.Adragon, "9/13/2021 5:10:27 PM")]
    public class ChainsOfJusticeProjectile : AscensionProjectile
    {
        int counter;
        bool shouldICount;
        NPC target;
        int i;
        float shootToX;
        float shootToY;
        float distance;
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.hostile = false;
            Projectile.DamageType = ModContent.GetInstance<UmbralDamageClass>();
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.01f;
            Projectile.scale = 0.75f;
            Projectile.damage = 35;
        }
        public override void AI()
        {
            Projectile.alpha -= 15;
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (counter == 0)
            {
                shouldICount = true;
            }
            if (shouldICount == true)
            {
                counter++;
            }
            for (int i = 0; i < 200; i++)
            {
                //Enemy NPC variable being set
                NPC target = Main.npc[i];

                //If the distance between the projectile and the live target is active
                if (distance < 480f && !target.friendly && target.active)
                {
                    shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                    shootToY = target.position.Y - Projectile.Center.Y;
                    break;
                }
            }
            if (counter == 45)
            {
                float projectileSpeed = 0;
                Vector2 velocity = Vector2.Normalize(new Vector2(Projectile.Center.X, Projectile.Center.Y)) * projectileSpeed;
                Projectile.velocity = velocity;
            }
            if (counter == 120)
            {
                float projectileSpeed = 12;
                Vector2 velocity = Vector2.Normalize(new Vector2(shootToX, shootToY)) * projectileSpeed;
                Projectile.velocity = velocity;
                shouldICount = false;
                counter = 1;
            }
        }
    }
}

