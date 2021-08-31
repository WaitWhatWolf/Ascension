using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources.Trademark;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/31 6:22:26")]
    public class Projectile_ParasiteSlimeArrow : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.width = 14;
            Projectile.height = 32;
            Projectile.arrow = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;

            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override void PostDraw(Color lightColor)
        {
            ASCResources.Dusts.Dust_ParasiteSlime_ProjTravel(Projectile).Create(Projectile.position);
        }

        public override void Kill(int timeLeft)
        {
            ASCResources.Dusts.Dust_ParasiteSlime_Explode.Create(Projectile.Center);
            Hooks.InGame.ApplyModBuffToAllWithin<Buff_Parasites>(Projectile, Projectile.Center, PARASITESLIME_EXP_RANGE, PARASITESLIME_BUFF_DURATION, ASCResources.Delegates.IsNotSlime);
        }
    }
}
