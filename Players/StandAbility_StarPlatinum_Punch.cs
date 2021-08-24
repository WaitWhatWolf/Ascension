using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using static Ascension.ASCResources.Stats;
using static Ascension.ASCResources.Textures;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class StandAbility_StarPlatinum_Punch : StandAbility
    {
        public override string Name { get; } = "Ora! Ora! Ora!";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title) + "'s bread & butter: Just punching.\nAttacks all nearby enemies repeatedly.\n"
            + Hooks.Colors.GetColoredTooltipText($"Attacks every {GetCooldown()}s.", Hooks.Colors.Tooltip_Stand_Ability_Cooldown);

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_STARPLATINUM_BASIC);

        public StandAbility_StarPlatinum_Punch(Stand stand) : base(stand)
        {
            pv_Owner = stand.Owner.Player;

            stand.SetBaseMovementAI(MovementAI);
        }

        protected override ReturnCountdown Countdown => pv_UTDCooldown;

        protected override bool ActivateCondition() => CountdownReady && pv_Target != null && pv_DistFromTarget <= pv_StandAttackRange;

        protected override bool DeactivateCondition() => Active;

        protected override void OnActivate()
        {
            Projectile projectile = Stand.GetStandProjectile();
            List<NPC> npcs = Hooks.InGame.GetAllWithin(projectile, projectile.Center, pv_StandAttackRange);
            foreach (NPC npc in npcs)
            {
                int damageAdder = pv_StandArmorPen > npc.defense ? npc.defense : pv_StandArmorPen;
                npc.StrikeNPC(projectile.damage + damageAdder, projectile.knockBack, projectile.direction);
            }

            ResetCooldown();
        }

        protected override void OnDeactivate() { }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_UTDCooldown = 60f / Stand.GetSingleStat(STAND_STAT_ATTACKSPEED);
            pv_StandArmorPen = Stand.GetStat(STAND_STAT_ARMORPEN);
            pv_StandAttackRange = Stand.GetSingleStat(STAND_STAT_ATTACKRANGE);
            pv_StandMoveSpeed = Stand.GetSingleStat(STAND_STAT_MOVESPEED);
            pv_StandAIRange = Stand.GetSingleStat(STAND_STAT_AIRANGE);
        }

        private void MovementAI()
        {
            Projectile projectile = Stand.GetStandProjectile();
            SearchForTargets(projectile, out bool foundTarget, out pv_DistFromTarget, out pv_Target);
            FinalizeAI(foundTarget, pv_DistFromTarget, pv_Target?.Center ?? pv_Owner.Center, projectile);
        }

        private void FinalizeAI(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, Projectile projectile)
        {
            float speed = 0f;
            float inertia = 0f;
            Vector2 finalPos = default;

            if (foundTarget)
            {
                finalPos = targetCenter;
                speed = distanceFromTarget <= pv_StandAttackRange ? pv_StandMoveSpeed / 4f : pv_StandMoveSpeed;
                inertia = speed * 2f;
            }
            else
            {
                finalPos = pv_Owner.Center + new Vector2(-pv_Owner.direction * 32f, -32f);
                float distFromOwner = pv_Owner.position.Distance(projectile.position);
                speed = pv_StandMoveSpeed / 2f;
                inertia = pv_StandMoveSpeed * 2f;

                if (distFromOwner > 600f)
                {
                    speed = inertia = pv_StandMoveSpeed * 2f;
                }
            }

            Vector2 destination = finalPos;

            Vector2 directionToTargetFromOwner = destination - pv_Owner.Center;
            directionToTargetFromOwner.Normalize();
            directionToTargetFromOwner *= (pv_StandAttackRange / 3f);
            if (foundTarget) destination -= directionToTargetFromOwner;

            Vector2 destDir = destination - projectile.Center;
            destDir.Normalize();
            destDir *= speed;
            projectile.velocity = (projectile.velocity * (inertia - 1) + destDir) / inertia;


            projectile.spriteDirection = foundTarget ? directionToTargetFromOwner.X > 0f ? -1 : 1 : -pv_Owner.direction;

        }

        private void SearchForTargets(Projectile projectile, out bool foundTarget, out float distanceFromTarget, out NPC target)
        {
            distanceFromTarget = pv_StandAIRange;
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

        private float pv_UTDCooldown;
        private int pv_StandArmorPen;
        private float pv_StandAttackRange;
        private float pv_StandAIRange;
        private float pv_StandMoveSpeed;
        private Player pv_Owner;
        private NPC pv_Target;
        private float pv_DistFromTarget;
    }
}
