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
        public override string Description => "Locates the {{c:Stat=strongest enemy}} on screen" +
            "\nand {{c:Effect=deals constant damage}} to it." +
            "\n\n{{c:Quote=I'll make you swiss cheese!}}";
        public override Asset<Texture2D> Icon => ASCResources.Textures.Default;

        protected override ReturnCountdown Countdown => pv_Countdown;

        protected override bool ActivateCondition() => Countdown && !pv_IsIdle;

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

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_StandDamage = Stand.GetDamage();
            pv_StandAttackRange = Stand.GetRange();
            pv_StandNPCDetectionRange = Stand.GetAIRange();
            pv_StandMoveSpeed = Stand.GetSpeed();
            pv_Countdown = 60f / Stand.GetAttackSpeed();
        }

        private void MovementAI()
        {
            Projectile standProj = Stand.GetStandProjectile();
            pv_IsIdle = Stand.Owner.Target == null || Stand.Owner.Target.Center.Distance(standProj.Center) <= pv_StandNPCDetectionRange;
            Vector2 rawDest = !pv_IsIdle ? Owner.Target.Center : pv_IdleMovesRight ? IdlePosRight : IdlePosLeft;
            Vector2 velocity = standProj.Center.DirectionTo(rawDest);
            velocity.Normalize();
            velocity *= pv_StandMoveSpeed * ASCResources.FLOAT_PER_FRAME;
            standProj.velocity += velocity;
            standProj.rotation = standProj.Center.AngleTo(rawDest);
            standProj.spriteDirection = standProj.Center.X > rawDest.X ? -1 : 1;

            float dist = standProj.Center.Distance(rawDest);
            if (pv_IsIdle && dist <= 5f)
                pv_IdleMovesRight = !pv_IdleMovesRight;
            else if (!pv_IsIdle && dist <= pv_StandAttackRange)
            {
                Vector2 toApply = Hooks.MathF.MoveTowards(standProj.velocity, Vector2.Zero, pv_StandMoveSpeed * ASCResources.FLOAT_PER_FRAME * 2f);
                standProj.velocity = toApply;
                //^I was too lazy to make better logic here^, so it will just move twice as fast towards immobile position, whatever
            }
        }

        private AscendedPlayer Owner => Stand.Owner;
        private Vector2 IdlePosRight => Owner.Player.Center + new Vector2(pv_IdlePositionRoam.X, pv_IdlePositionRoam.Y);
        private Vector2 IdlePosLeft => Owner.Player.Center + new Vector2(-pv_IdlePositionRoam.X, pv_IdlePositionRoam.Y);

        private Vector2 pv_IdlePositionRoam = new(48f, -72f);
        private bool pv_IdleMovesRight;
        private bool pv_IsIdle;

        private ReturnCountdown pv_Countdown;
        private int pv_StandDamage;
        private float pv_StandAttackRange;
        private float pv_StandNPCDetectionRange;
        private float pv_StandMoveSpeed;

        private Dust pv_LineDrawer;
    }
}
