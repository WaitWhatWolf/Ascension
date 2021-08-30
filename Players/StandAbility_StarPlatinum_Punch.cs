using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using static Ascension.ASCResources.Stats;
using static Ascension.ASCResources.Textures;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class StandAbility_StarPlatinum_Punch : StandAbility_DefaultMovement
    {
        public override string Name { get; } = "Ora! Ora! Ora!";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title) + "'s bread & butter: Just punching.\nAttacks all nearby enemies repeatedly.\n"
            + Hooks.Colors.GetColoredTooltipText($"Attacks every {GetCooldown()}s.", Hooks.Colors.Tooltip_Stand_Ability_Cooldown);

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_STARPLATINUM_BASIC);

        public StandAbility_StarPlatinum_Punch(Stand stand) : base(stand)
        {

        }

        protected override ReturnCountdown Countdown => pv_UTDCooldown;

        protected override bool ActivateCondition() => CountdownReady && StandMoveTarget != null && StandMoveTargetDist(true) <= pv_StandAttackRange;

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

            ResetCountdown();
        }

        protected override void OnDeactivate() { }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_UTDCooldown = 60f / Stand.GetAttackSpeed();
            pv_StandArmorPen = Stand.GetArmorPen();
            pv_StandAttackRange = Stand.GetRange();
            pv_StandMoveSpeed = Stand.GetSpeed();
            pv_StandNPCDetectionRange = Stand.GetAIRange();
        }

        protected override float StandMoveAttackRange => pv_StandAttackRange;
        protected override float StandMoveNPCDetectionRange => pv_StandNPCDetectionRange;
        protected override float StandMoveSpeed => pv_StandMoveSpeed;

        private ReturnCountdown pv_UTDCooldown;
        private int pv_StandArmorPen;
        private float pv_StandAttackRange;
        private float pv_StandNPCDetectionRange;
        private float pv_StandMoveSpeed;
    }
}
