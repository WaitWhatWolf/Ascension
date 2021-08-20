using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Ascension.Items;
using Terraria.ID;
using Ascension.Internal;
using Ascension.Utility;
using Ascension.Buffs;
using Microsoft.Xna.Framework;

namespace Ascension.Projectiles
{
    /// <summary>
    /// Thrown by <see cref="Item_ParasiteSlimeJavelin"/>.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/11 18:06:15")]
    public class Projectile_ParasiteSlimeJavelin : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.JavelinFriendly);

            Projectile.width = 60;
            Projectile.height = 20;
            Projectile.penetrate = 1;

            Projectile.DamageType = ModContent.GetInstance<UmbralDamageClass>();
        }

        public override void PostAI()
        {
            if (Hooks.MathF.ProcessCountdown(ref counter, 0.2f))
                MakeDust();
        }

        public override void Kill(int timeLeft)
        {
            Hooks.InGame.ApplyModBuffToAllWithin<Buff_Parasites>(this, Projectile.Center, 70f, 70);
            MakeDust(5, 2.5f);
        }

        private void MakeDust(int multiplier = 1, float scale = 1f)
        {
            IntRange range = new(1 * multiplier, 6 * multiplier);
            Vector2Range pos = new(Projectile.Top.X - 5f, Projectile.Top.Y - 5f, Projectile.Top.X + 5f, Projectile.Top.Y + 5f);
            
            Hooks.InGame.CreateDust(DustID.t_Slime, range, pos, 5, 5, new(-0.1f, 0.1f), scale == 1f ? 1f : 0f, new(0, 100), scale, Event_OnDustCreate);
        }

        private void Event_OnDustCreate(Dust dust)
        {
            dust.velocity *= 2f;
            dust.noGravity = dust.scale != 1f;
        }

        private float counter = 0.2f;
    }
}
