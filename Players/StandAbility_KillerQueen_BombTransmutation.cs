using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using static Ascension.ASCResources.Textures;
using static Ascension.ASCResources.Stats;
using Microsoft.Xna.Framework;
using Ascension.NPCs;
using Ascension.Buffs.StandUnique;
using System.Linq;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 7:02:06")]
    public class StandAbility_KillerQueen_BombTransmutation : StandAbility_DefaultMovement
    {
        public StandAbility_KillerQueen_BombTransmutation(Stand stand) : base(stand)
        {
        }

        public override string Name { get; } = "Bomb Transmutation";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Killer Queen", Hooks.Colors.Tooltip_Stand_Title) + 
            " applies a bomb to the nearest target\nwhich detonates after " + 
            Hooks.Colors.GetColoredTooltipText(pv_DetonateCountdown.Countdown.Truncate(2).ToString() + 's', Hooks.Colors.Tooltip_Delay)
            + "\n\n"
            + Hooks.Colors.GetColoredTooltipText("I have no choice but to fight the ones who find out my identity.", Hooks.Colors.Tooltip_Quote);

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_KILLERQUEEN_BASIC);

        public override void Update()
        {
            base.Update();


        }

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => CountdownReady && StandMoveTarget != null && StandMoveTargetDist <= pv_StandAttackRange;
        protected override bool DeactivateCondition() => Active;

        protected override void OnActivate()
        {
            if(StandMoveTarget.TryGetGlobalNPC<StandHandlerNPC>(out StandHandlerNPC standHandler) && !standHandler.Debuffs.Any(d => d is SB_KillerQueen_Bomb))
            {
                standHandler.AddDebuff(new SB_KillerQueen_Bomb(Stand, pv_DetonateCountdown, pv_StandDamage, pv_BombPen, pv_BombKnockback));
            }

            ResetCooldown();
        }

        protected override void OnDeactivate()
        {

        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_Countdown = 60f / Stand.GetSingleStat(STAND_STAT_ATTACKSPEED);
            pv_DetonateCountdown = pv_Countdown.Countdown / 2f;
            pv_StandDamage = Stand.GetStat(STAND_STAT_DAMAGE);
            pv_BombPen = Stand.GetStat(STAND_STAT_ARMORPEN);
            pv_StandAttackRange = Stand.GetSingleStat(STAND_STAT_ATTACKRANGE);
            pv_StandMoveSpeed = Stand.GetSingleStat(STAND_STAT_MOVESPEED);
            pv_StandNPCDetectionRange = Stand.GetSingleStat(STAND_STAT_AIRANGE);
            pv_BombKnockback = Stand.GetSingleStat(STAND_STAT_KNOCKBACK);
        }

        protected override float StandMoveAttackRange => pv_StandAttackRange;
        protected override float StandMoveNPCDetectionRange => pv_StandNPCDetectionRange;
        protected override float StandMoveSpeed => pv_StandMoveSpeed;

        private ReturnCountdown pv_Countdown;
        private ReturnCountdown pv_DetonateCountdown;
        private int pv_StandDamage;
        private int pv_BombPen;
        private float pv_StandAttackRange;
        private float pv_StandNPCDetectionRange;
        private float pv_BombKnockback;
        private float pv_StandMoveSpeed;
    }
}
