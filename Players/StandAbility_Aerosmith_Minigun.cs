using Ascension.Attributes;
using Ascension.Dusts;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Players
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/29 9:10:52")]
    public class StandAbility_Aerosmith_Minigun : StandAbility
    {
        public StandAbility_Aerosmith_Minigun(Stand stand, int index) : base(stand, index)
        {
            stand.SetBaseMovementAI(MovementAI);
        }

        public override string Name => "Minigunning";
        public override string Description => "Locates the {{c:Stat=strongest enemy}} on screen and {{c:Effect=deals constant damage}} to it." +
            "\n{{c:Quote=I'll make you swiss cheese!}}";
        public override Asset<Texture2D> Icon => ASCResources.Textures.Default;

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => Countdown && Stand.Owner.Target != null;

        protected override bool DeactivateCondition() => true;

        protected override void OnActivate()
        {
            Stand.Owner.Target.StrikeNPC(pv_StandDamage, 1f, Hooks.InGame.GetKnockbackDirection(Stand.GetStandProjectile().Center, Stand.Owner.Target.Center));

            pv_LineDrawer = Main.dust[Dust.NewDust(Stand.GetStandProjectile().Center, 1, 1, ModContent.DustType<Dust_LineDrawer>(), newColor: Color.Yellow, Alpha: 200)];

            SetLineDrawerData();
        }

        protected override void OnDeactivate()
        {
            pv_LineDrawer.active = false;
            pv_LineDrawer = null;
        }

        private void SetLineDrawerData()
        {
            Vector2 eyePos = Stand.GetStandProjectile().Bottom;
            Vector2 safeTargetPos = (Stand.Owner.Target?.position ?? eyePos);
            pv_LineDrawer.customData = (eyePos, safeTargetPos.DirectionFrom(eyePos).ToRotation(), Math.Min((int)safeTargetPos.Distance(eyePos), 150), 2);
            pv_LineDrawer.active = true;
        }

        private void MovementAI()
        {
            //TODO
        }

        private ReturnCountdown pv_Countdown;
        private int pv_StandDamage;
        private float pv_StandAttackRange;
        private float pv_StandNPCDetectionRange;
        private float pv_StandMoveSpeed;

        private Dust pv_LineDrawer;
    }
}
