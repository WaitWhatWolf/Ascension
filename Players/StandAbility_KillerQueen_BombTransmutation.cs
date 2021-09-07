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
        public StandAbility_KillerQueen_BombTransmutation(Stand stand, int index) : base(stand, index)
        {
        }

        public override string Name { get; } = "Bomb Transmutation";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Killer Queen", Hooks.Colors.Tooltip_Stand_Title) +
            " applies a bomb to the most recently attacked target." +
            "\nHe then detonates the bomb after a " + Hooks.Colors.GetColoredTooltipText(pv_DetonateCountdown.Countdown.Truncate(2).ToString() + 's', Hooks.Colors.Tooltip_Delay) 
            + " delay.";

        protected override bool DisplaysCountdownOnTooltip { get; } = false;
        public override string Quote { get; } = "I have no choice but to fight the ones who find out my identity.";

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_KILLERQUEEN_BASIC);

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => CountdownReady && StandMoveTargetFound && StandMoveTargetDist(true) <= pv_StandAttackRange;
        protected override bool DeactivateCondition() => Active;

        protected override void OnActivate()
        {
            if(StandMoveTarget.TryGetGlobalNPC(out StandHandlerNPC standHandler) && !standHandler.Debuffs.Any(d => d is SB_KillerQueen_Bomb))
            {
                standHandler.AddDebuff(new SB_KillerQueen_Bomb(Stand, pv_DetonateCountdown, pv_StandDamage, pv_BombPen, pv_BombKnockback));
                Stand.StandAnimator.Speed = Stand.GetAttackSpeed() / 10f;
                Stand.StandAnimator.Play(ASCResources.Animations.NAME_STAND_KILLERQUEEN_PLACEBOMB);
            }

            ResetCountdown();
        }

        protected override void OnDeactivate()
        {
        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_Countdown = 60f / Stand.GetAttackSpeed();
            pv_DetonateCountdown = pv_Countdown.Countdown / 2f;
            pv_StandDamage = Stand.GetDamage();
            pv_BombPen = Stand.GetArmorPen();
            pv_StandAttackRange = Stand.GetRange();
            pv_StandMoveSpeed = Stand.GetSpeed();
            pv_StandNPCDetectionRange = Stand.GetAIRange();
            pv_BombKnockback = Stand.GetKnockback();
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
