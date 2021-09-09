using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles;
using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Ascension.Buffs;
using Ascension.Dusts;
using Ascension.Players;
using Ascension.Utility;
using System;
using System.Collections.Generic;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.Adragon, "9/9/2021 6:11:54 PM")]
    public class LineChain : AscensionProjectile
    {
        float projectileSpeed;
        public override void SetDefaults()
        {
            Projectile.width = 742;
            Projectile.height = 43;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 80;
            Projectile.tileCollide = false;
            Projectile.aiStyle = 0;
            Projectile.knockBack = 0.05f;
            Projectile.scale = 0.8f;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        int i = 0;
        public override void AI()
        {
            i++;
            if(i >= 40)
            {
                Player player = Main.player[Projectile.owner];
                float ProjectileSpeed = -10;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * ProjectileSpeed;
                Projectile.velocity = velocity;
            }
            else
            {
                Player player = Main.player[Projectile.owner];
                float ProjectileSpeed = 0;
                Vector2 velocity = Vector2.Normalize(new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2) -
                new Vector2(Projectile.position.X + Projectile.width, Projectile.position.Y + Projectile.height)) * ProjectileSpeed;
                Projectile.velocity = velocity;
            }
            Projectile.rotation += 0.1f;

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            //SoundEngine.PlaySound(SoundID.Item40, Projectile.position);
        }
    }
}
