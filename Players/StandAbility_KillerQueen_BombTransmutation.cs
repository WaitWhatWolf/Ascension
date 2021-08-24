using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using static Ascension.ASCResources.Textures;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 7:02:06")]
    public class StandAbility_KillerQueen_BombTransmutation : StandAbility
    {
        public StandAbility_KillerQueen_BombTransmutation(Stand stand) : base(stand)
        {
            pv_Owner = stand.Owner.Player;

            stand.SetBaseMovementAI(MovementAI);
        }

        public override string Name { get; } = "Bomb Transmutation";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Killer Queen", Hooks.Colors.Tooltip_Stand_Title) + 
            " applies a bomb to the nearest target\nwhich detonates after " + 
            Hooks.Colors.GetColoredTooltipText(pv_DetonateCountdown.Countdown.Truncate(2).ToString() + 's', Hooks.Colors.Tooltip_Delay)
            + "\n\n"
            + Hooks.Colors.GetColoredTooltipText("I have no choice but to fight the ones who find out my identity.", Hooks.Colors.Tooltip_Quote);

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_KILLERQUEEN_BASIC);

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => false;

        protected override bool DeactivateCondition() => false;

        protected override void OnActivate()
        {

        }

        protected override void OnDeactivate()
        {

        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_Countdown = 60f / Stand.GetSingleStat(STAND_STAT_ATTACKSPEED);
            pv_DetonateCountdown = pv_Countdown.Countdown / 2f;
        }

        private void MovementAI()
        {

        }

        private ReturnCountdown pv_Countdown;
        private ReturnCountdown pv_DetonateCountdown;

        private Player pv_Owner;
    }
}
