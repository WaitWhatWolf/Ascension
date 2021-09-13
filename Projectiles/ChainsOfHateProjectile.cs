using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Internal;
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
    [CreatedBy(Dev.Adragon, "9/13/2021 4:54:50 PM")]
    public class ChainsOfHateProjectile : AscensionProjectile
    {
        int counter;
        bool shouldICount;
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
            if (counter > 44)
            {
                /*
                Player player = Main.player[Projectile.owner];
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float projectileSpeed = 0;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * projectileSpeed;
                Projectile.velocity = velocity;
                */
            }
            if (counter == 45)
            {
                Player player = Main.player[Projectile.owner];
                float projectileSpeed = 0;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * projectileSpeed;
                Projectile.velocity = velocity;
            }
            if (counter == 120)
            {
                Player player = Main.player[Projectile.owner];
                float projectileSpeed = 15;

                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * projectileSpeed;

                Projectile.velocity = velocity;
                shouldICount = false;
                counter = 1;
            }
        }

    }
}
