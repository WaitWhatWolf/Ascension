using Ascension.Attributes;
using Ascension.Buffs;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 11:31:53")]
    public class StandAbility_KillerQueen_SheerHeartAttack : StandAbility, IAbilityHideCountdown
    {
        public StandAbility_KillerQueen_SheerHeartAttack(Stand stand) : base(stand) 
        {
            stand.OnStandRecall += Event_OnStandRecall;
        }

        public override string Name { get; } = "Sheer Heart Attack";

        [Note(Dev.WaitWhatWolf, "Yes, the dot at the end is necessary, again, excuse my OCD.")]
        public override string Description => Hooks.Colors.GetColoredTooltipText(Stand.Owner.Player.name, Hooks.Colors.Tooltip_Player_Title)
            + " detaches " + (Stand.Owner.Player.Male ? "his" : "her") + " right hand, which " + Hooks.Colors.GetColoredTooltipText("drastically reduces attack speed", Hooks.Colors.Tooltip_Debuff)
            + ".\nThe detached hand turns into " + Hooks.Colors.GetColoredTooltipText(Name, Hooks.Colors.Tooltip_Stand_Ability)
            + "\nwhich "+ Hooks.Colors.GetColoredTooltipText("autonomously seeks", Hooks.Colors.Tooltip_Effect) 
            + " the target with the " + Hooks.Colors.GetColoredTooltipText("highest health", Hooks.Colors.Tooltip_Stat)
            + ",\nthen exploding it after a short delay, with damage equal" 
            + "\n to a portion of the enemy's " + Hooks.Colors.GetColoredTooltipText("current health", Hooks.Colors.Tooltip_Stat)
            + '.';

        public override Asset<Texture2D> Icon => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_ABILITY_KILLERQUEEN_ABILITY2);

        public override void Update()
        {
            base.Update();

            if(Active && !pv_CanRecall && pv_RecallCountdown)
            {
                pv_CanRecall = true;
            }
        }

        protected override ReturnCountdown Countdown { get; } = 1f;

        protected override bool ActivateCondition() => CountdownReady && !Active;

        protected override bool DeactivateCondition() => !Stand.Active || (ASCResources.Input.GetStandAbilityKey(2).Current && pv_CanRecall) || pv_SheerHeartAttack == null;

        protected override void OnActivate()
        {
            Countdown.Reset();
            int projIndex = Projectile.NewProjectile(new ProjectileSource_Stand(Stand.Owner, Stand), Stand.Owner.Player.Center, new(0, 1f), ModContent.ProjectileType<Projectile_SheerHeartAttack>(), Stand.GetDamage(), 0);
            pv_SheerHeartAttack = ((Projectile_SheerHeartAttack)Main.projectile[projIndex].ModProjectile);
            pv_SheerHeartAttack.Init(Stand);
        }

        protected override void OnDeactivate()
        {
            pv_SheerHeartAttack?.Deinit();
            pv_SheerHeartAttack = null;
            pv_RecallCountdown.Reset();
            pv_CanRecall = false;
        }

        private void Event_OnStandRecall()
        {
            OnDeactivate();
            Debug.Log(pv_SheerHeartAttack);
        }

        bool IAbilityHideCountdown.HideCountdown() => true;

        private ReturnCountdown pv_RecallCountdown = 0.2f;
        private bool pv_CanRecall;
        private Projectile_SheerHeartAttack pv_SheerHeartAttack;
    }
}
