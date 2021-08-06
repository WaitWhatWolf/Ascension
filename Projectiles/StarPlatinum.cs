using Ascension.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Ascension.ASCResources.Stats;

namespace Ascension.Projectiles
{
    public sealed class StarPlatinum : StandProjectile
    {
        /// <inheritdoc/> //Not sure what to put as desc here lol
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.DamageType = ASCResources.Stats.DamageClass_Umbral;
            base.SetDefaults();
        }

        public override bool CloneNewInstances => true;

        public override void AI()
        {
            if (!CheckActive(pv_Owner))
                return;

            GeneralBehavior(out Vector2 vectorToIdlePos, out float distToIdlePos);
            SearchForTargets(out bool foundTarget, out float distFromTarget, out NPC target);
            FinalizeAI(foundTarget, target, distFromTarget, target?.Center ?? pv_Owner.Center, distToIdlePos, vectorToIdlePos);
        }

        private void GeneralBehavior(out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = pv_Owner.Center;
            idlePosition.Y -= 24f; // Move up one and a half tiles

            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -pv_Owner.direction; //Basically makes the stand go behind the player.
            idlePosition.X += minionPositionOffsetX;

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == pv_Owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }
        }

        private void SearchForTargets(out bool foundTarget, out float distanceFromTarget, out NPC target)
        {
            distanceFromTarget = pv_StandAIRange;
            foundTarget = false;
            target = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy(this, true))
                {
                    float between = Vector2.Distance(npc.Center, Projectile.Center);
                    bool inRange = between < distanceFromTarget;
                    
                    if (inRange)
                    {
                        distanceFromTarget = between;
                        foundTarget = true;
                        target = npc;
                    }
                }
            }
        }

        private void FinalizeAI(bool foundTarget, NPC target, float distanceFromTarget, Vector2 targetCenter, 
            float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float speed = pv_StandMoveSpeed;
            float inertia = pv_StandMoveSpeed / 4f;

            if (foundTarget)
            {
                if (distanceFromTarget > 20f)
                {
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }

                if(distanceFromTarget < 40f)
                {
                    if(pv_AttackCooldown <= 0)
                    {
                        int damageAdder = pv_StandArmorPen > target.defense ? target.defense : pv_StandArmorPen;
                        target.StrikeNPC(Projectile.damage + damageAdder, Projectile.knockBack, Projectile.direction);
                        pv_AttackCooldown = 60f / pv_Stand.GetSingleStat(STAND_STAT_ATTACKSPEED);
                    }
                }
            }
            else
            {
                // Minion doesn't have a target: return to player and idle
                if (distanceToIdlePosition > 600f)
                {
                    // Speed up the minion if it's away from the player
                    speed = pv_StandMoveSpeed * 2f;
                    inertia = pv_StandMoveSpeed * 4f;
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = pv_StandMoveSpeed / 2f;
                    inertia = pv_StandMoveSpeed;
                }

                if (distanceToIdlePosition > 20f)
                {
                    // The immediate range around the player (when it passively floats about)

                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else if (Projectile.velocity == Vector2.Zero)
                {
                    // If there is a case where it's not moving at all, give it a little "poke"
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }

            pv_AttackCooldown -= ASCResources.FLOAT_PER_FRAME;
        }

        protected override void OnBossDefeated(string name)
        {
            base.OnBossDefeated(name);

            pv_StandArmorPen = pv_Stand.GetStat(STAND_STAT_ARMORPEN);

            pv_StandMoveSpeed = pv_Stand.GetSingleStat(STAND_STAT_MOVESPEED);
            pv_StandAIRange = pv_Stand.GetSingleStat(STAND_STAT_AIRANGE);
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
                return false;

            return true;
        }

        private float pv_StandAIRange;
        private float pv_AttackCooldown;
        private int pv_StandArmorPen;
        private float pv_StandMoveSpeed;
    }
}
