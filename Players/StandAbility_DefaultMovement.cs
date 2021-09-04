using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 13:34:03")]
    public abstract class StandAbility_DefaultMovement : StandAbility, IAbilityHideCountdown
    {
        public StandAbility_DefaultMovement(Stand stand) : base(stand)
        {
            pr_Owner = stand.Owner.Player;

            stand.SetBaseMovementAI(MovementAI);
            stand.Owner.OnSetTarget += Event_OnSetTarget;
            stand.Owner.OnSameTarget += Event_OnSetTarget;
            stand.Owner.OnRemoveTarget += Event_OnRemoveTarget;
        }

        protected Player pr_Owner;
        protected NPC StandMoveTarget;
        protected bool StandMoveTargetFound;
        protected abstract float StandMoveAttackRange { get; }
        protected abstract float StandMoveNPCDetectionRange { get; }
        protected abstract float StandMoveSpeed { get; }
        protected float StandMoveTargetDist(bool fromProjectile) => StandMoveTargetFound ? StandMoveTarget.Center.Distance(fromProjectile ? Stand.GetStandModProjectile().Front : pr_Owner.Center) : float.PositiveInfinity;

        private void Event_OnSetTarget(NPC obj)
        {
            StandMoveTarget = obj;
            StandMoveTargetFound = true;
        }

        private void Event_OnRemoveTarget(NPC obj)
        {
            StandMoveTarget = null;
            StandMoveTargetFound = false;
        }

        private void MovementAI()
        {
            Projectile projectile = Stand.GetStandProjectile();

            FinalizeAI(StandMoveTargetDist(true), StandMoveTarget?.Center ?? pr_Owner.Center, projectile);
        }

        private void FinalizeAI(float distanceFromTarget, Vector2 targetCenter, Projectile projectile)
        {
            float speed;
            float inertia;
            Vector2 finalPos;
            bool goesToTarget = StandMoveTargetFound && StandMoveTargetDist(false) <= StandMoveNPCDetectionRange;
            
            if (goesToTarget)
            {
                finalPos = targetCenter;
                speed = distanceFromTarget <= StandMoveAttackRange ? StandMoveSpeed / 2f : StandMoveSpeed;
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
            if (StandMoveTargetFound) destination -= directionToTargetFromOwner;

            Vector2 destDir = destination - Stand.GetStandModProjectile().Front;
            destDir.Normalize();
            destDir *= speed;
            projectile.velocity = (projectile.velocity * (inertia - 1) + destDir) / inertia;


            projectile.spriteDirection = goesToTarget ? directionToTargetFromOwner.X > 0f ? -1 : 1 : -pr_Owner.direction;

        }

        bool IAbilityHideCountdown.HideCountdown() => true;
    }
}
