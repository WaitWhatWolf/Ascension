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
        public StandAbility_KillerQueen_SheerHeartAttack(Stand stand, int index) : base(stand, index) 
        {
            stand.OnStandRecall += Event_OnStandRecall;

            CanSeekCountdown = new(60f / (stand.GetAttackSpeed() / 5f), false);
        }

        public override string Name { get; } = "Sheer Heart Attack";

        [Note(Dev.WaitWhatWolf, "Yes, the dot at the end is necessary, again, excuse my OCD.")]

        public override string Description =>
            $"{{{{c:Player_Title={Stand.Owner.Player.name}}}}} detaches {(Stand.Owner.Player.Male ? "his" : "her")}"
            + " right hand, which {{c:Debuff=drastically reduces attack speed}}."
            + $"\nThe detached hand turns into {{{{c:Stand_Ability={Name}}}}}"
            + "\nwhich {{c:Effect=autonomously seeks}} the target with the {{c:Stat=highest health}},"
            + "\napplying a bomb on the target when near."
            + "\nAfter a {{c:Delay=short delay}}, the bomb {{c:Effect=explodes}} with damage equal"
            + "\nto {{c:Stat=50% of it's current health}}.";

        public override string Quote { get; } = "Sheer Heart Attack has no weaknesses.";

        public override Asset<Texture2D> Icon => ASCResources.Textures.GetTexture(ASCResources.Textures.STAND_ABILITY_KILLERQUEEN_ABILITY2);

        public override void Update()
        {
            base.Update();

            if(Active && !pv_CanRecall && pv_RecallCountdown)
            {
                pv_CanRecall = true;
            }
        }

        internal ReturnCountdown CanSeekCountdown { get; private set; }
        internal bool CanSeekTarget;

        protected override ReturnCountdown Countdown { get; } = 1f;

        protected override bool ActivateCondition() => CountdownReady && !Active;

        protected override bool DeactivateCondition() => !Stand.Active || (ASCResources.Input.GetStandAbilityKey(Index).Current && pv_CanRecall) || pv_SheerHeartAttack == null;

        protected override void OnActivate()
        {
            ResetCountdown();
            int projIndex = Projectile.NewProjectile(new ProjectileSource_Stand(Stand.Owner, Stand), Stand.Owner.Player.Top, new(0, 1f), ModContent.ProjectileType<Projectile_SheerHeartAttack>(), Stand.GetDamage(), 0);
            pv_SheerHeartAttack = ((Projectile_SheerHeartAttack)Main.projectile[projIndex].ModProjectile);
            pv_SheerHeartAttack.Init(Stand, this);
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
        }

        bool IAbilityHideCountdown.HideCountdown() => true;

        private ReturnCountdown pv_RecallCountdown = 0.2f;
        private bool pv_CanRecall;
        private Projectile_SheerHeartAttack pv_SheerHeartAttack;
    }
}
