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
            if (pv_DustCountdown)
                pv_DustMTravel.CreateDust();
        }

        public override void Kill(int timeLeft)
        {
            Hooks.InGame.ApplyModBuffToAllWithin<Buff_Parasites>(this, Projectile.Center, 70f, 70, npc => !npc.FullName.ToUpper().Contains("SLIME"));
            pv_DustMExplosion.CreateDust();
        }

        private void Event_OnDustCreate(Dust dust)
        {
            dust.velocity *= 2f;
            dust.noGravity = dust.scale != 1f;
        }

        private Vector2Range Func_DustPos() => new(Projectile.Bottom, Projectile.Top);

        public Projectile_ParasiteSlimeJavelin()
        {
            pv_DustMExplosion = new DustMaker(new(20, 25), Func_DustPos, 5, 5, Color.DodgerBlue, new(-1f, 1f), new(1f, -0.3f), new(0, 100), new(1.2f, 1.6f), Event_OnDustCreate, DustID.t_Slime, DustID.BunnySlime, DustID.Crimslime);
            pv_DustMTravel = new DustMaker(new(4, 8), Func_DustPos, 5, 5, Color.DodgerBlue, new(-3f, 3f), new(1f, 0.3f), new(0, 100), new(0.9f, 1f), Event_OnDustCreate, DustID.t_Slime, DustID.BunnySlime, DustID.Crimslime);
        }

#pragma warning disable IDE0044
        private DustMaker pv_DustMTravel;
        private DustMaker pv_DustMExplosion;

        private ReturnCountdown pv_DustCountdown = 0.1f;
#pragma warning restore IDE0044
    }
}
