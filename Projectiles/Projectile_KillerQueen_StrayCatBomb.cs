using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/29 6:32:56")]
    public class Projectile_KillerQueen_StrayCatBomb : AscensionProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 500;
            Projectile.friendly = true;
            Projectile.scale = 0.5f;
            Projectile.alpha = 125;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Melee;
        }

        public override bool MinionContactDamage() => true;

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            Projectile.alpha -= 3;
            Projectile.scale = Math.Min(Projectile.scale + FLOAT_PER_FRAME, 1f);

            Projectile.velocity *= (1f - (FLOAT_PER_FRAME * 2f));
            if(pv_HasTarget)
            {
                float dist = Vector2.Distance(Projectile.Center, pv_Target.Center);
                if(dist <= pv_Range)
                    Projectile.velocity = (pv_Target.Center-Projectile.Center).SafeNormalize(Vector2.Zero) * pv_Speed;
                if (dist <= pv_ExplodeRange)
                    Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(timeLeft <= 0 ? SoundID.Item54 : SoundID.Item14, Projectile.position);

            pv_Player.OnSetTarget -= Event_OnSetTarget;
            pv_Player.OnSameTarget -= Event_OnSetTarget;
            pv_Player.OnRemoveTarget -= Event_OnRemoveTarget;

            ASCResources.Dusts.Dust_Stand_KillerQueen_Bubble.Create(Projectile.position);
            if(timeLeft > 0) CreateExplosion();
        }

        private void CreateExplosion()
        {
            ASCResources.Dusts.Gore_Stand_KillerQueen_Explosion.Create(Projectile.position);
            ASCResources.Dusts.Dust_Stand_KillerQueen_Explosion.Create(Projectile.position);
            foreach(NPC npc in Hooks.InGame.GetAllWithin(Projectile, Projectile.Center, pv_ExplodeRange * 2f))
            {
                npc.StrikeNPC(Projectile.damage, Projectile.knockBack, Projectile.direction);
            }
        }

        internal void Init(Stand stand, AscendedPlayer player)
        {
            SoundEngine.PlaySound(SoundID.Item85, Projectile.position);
            
            pv_Stand = stand;
            pv_Player = player;
            pv_Speed = pv_Stand.GetOtherSingleStat(ASCResources.Stats.STAND_STAT_PROJECTILE_VELOCITY);
            pv_Range = pv_Stand.GetAIRange() / 2f;
            pv_ExplodeRange = pv_Stand.GetRange();

            pv_Player.OnSetTarget += Event_OnSetTarget;
            pv_Player.OnSameTarget += Event_OnSetTarget;
            pv_Player.OnRemoveTarget += Event_OnRemoveTarget;

            if (pv_Player.Target != null)
                Event_OnSetTarget(pv_Player.Target);
        }

        private void Event_OnRemoveTarget(NPC obj)
        {
            pv_Target = null;
            pv_HasTarget = false;
        }

        private void Event_OnSetTarget(NPC obj)
        {
            pv_Target = obj;
            pv_HasTarget = true;
            if(obj.Center.Distance(Projectile.Center) <= pv_Range)
                Projectile.timeLeft = 400;
        }

        private AscendedPlayer pv_Player;
        private Stand pv_Stand;
        private float pv_Speed;
        private float pv_Range;
        private float pv_ExplodeRange;
        private bool pv_HasTarget;
        private NPC pv_Target;
    }
}
