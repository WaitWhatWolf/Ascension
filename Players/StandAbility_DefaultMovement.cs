using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using Terraria;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 13:34:03")]
    public abstract class StandAbility_DefaultMovement : StandAbility
    {
        public StandAbility_DefaultMovement(Stand stand) : base(stand)
        {
            pr_Owner = stand.Owner.Player;

            stand.SetBaseMovementAI(MovementAI);
        }

        private void MovementAI()
        {
            Projectile projectile = Stand.GetStandProjectile();
            SearchForTargets(projectile, out bool foundTarget, out StandMoveTargetDist, out StandMoveTarget);
            FinalizeAI(foundTarget, StandMoveTargetDist, StandMoveTarget?.Center ?? pr_Owner.Center, projectile);
        }

        private void FinalizeAI(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, Projectile projectile)
        {
            float speed;
            float inertia;
            Vector2 finalPos;

            if (foundTarget)
            {
                finalPos = targetCenter;
                speed = distanceFromTarget <= StandMoveAttackRange ? StandMoveSpeed / 4f : StandMoveSpeed;
                inertia = speed * 2f;
            }
            else
            {
                finalPos = pr_Owner.Center + new Vector2(-pr_Owner.direction * 32f, -32f);
                float distFromOwner = pr_Owner.position.Distance(projectile.position);
                speed = StandMoveSpeed / 2f;
                inertia = StandMoveSpeed * 2f;

                if (distFromOwner > 600f)
                {
                    speed = inertia = StandMoveSpeed * 2f;
                }
            }

            Vector2 destination = finalPos;

            Vector2 directionToTargetFromOwner = destination - pr_Owner.Center;
            directionToTargetFromOwner.Normalize();
            directionToTargetFromOwner *= (StandMoveAttackRange / 3f);
            if (foundTarget) destination -= directionToTargetFromOwner;

            Vector2 destDir = destination - projectile.Center;
            destDir.Normalize();
            destDir *= speed;
            projectile.velocity = (projectile.velocity * (inertia - 1) + destDir) / inertia;


            projectile.spriteDirection = foundTarget ? directionToTargetFromOwner.X > 0f ? -1 : 1 : -pr_Owner.direction;

        }

        private void SearchForTargets(Projectile projectile, out bool foundTarget, out float distanceFromTarget, out NPC target)
        {
            distanceFromTarget = StandMoveNPCDetectionRange;
            foundTarget = false;
            target = null;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (npc.CanBeChasedBy(this, true))
                {
                    float between = Vector2.Distance(npc.Center, projectile.Center);
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

        protected Player pr_Owner;
        protected abstract float StandMoveAttackRange { get; }
        protected abstract float StandMoveNPCDetectionRange { get; }
        protected abstract float StandMoveSpeed { get; }
        protected float StandMoveTargetDist;
        protected NPC StandMoveTarget;
    }
}
