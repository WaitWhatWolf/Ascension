using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using static Ascension.ASCResources;
using static Ascension.ASCResources.Stats;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 10),
        Note(Dev.WaitWhatWolf, "This ability is inspired by the Jotaro VS Steely Dan scene; " +
        "Named it \"Receipt\" cause it the class name is already getting long at this point.")]
    public sealed class StandAbility_StarPlatinum_Receipt : StandAbility
    {
        public override string Name { get; } = "Prime-time Delivery";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title) 
            + " teleports to it's user, after which\n"
            + "he dashes and deals substantial damage\n"
            + "to each enemy he passes by.\n"
            + Hooks.Colors.GetColoredTooltipText($"Cooldown: {GetCooldown()}", Hooks.Colors.Tooltip_Stand_Ability_Cooldown)
            + "\n\n"
            + Hooks.Colors.GetColoredTooltipText("Here's Your Receipt.", Hooks.Colors.Tooltip_Quote);

        public override Asset<Texture2D> Icon => ASCResources.Textures.Stand_Ability_StarPlatinum_Receipt;

        public StandAbility_StarPlatinum_Receipt(Stand stand) : base(stand)
        {

        }

        protected override float Cooldown { get; } = 12f;

        protected override bool ActivateCondition() => CooldownReady;

        protected override bool DeactivateCondition() => pv_CurrentDuration <= 0f && Active;

        protected override void OnActivate()
        {
            pv_CurrentDuration = pv_Duration;

            pv_StarPlatinum = Stand.GetStandModProjectile() as StarPlatinum;
            pv_StandProjectile = Stand.GetStandProjectile();
            pv_Owner = Stand.Owner.Player;

            pv_StandProjectile.netUpdate = true;
            pv_StandProjectile.Center = pv_Owner.Center;
            pv_Direction = new(pv_Owner.direction * pv_StandMovementSpeed, 0f);

            Stand.AddMovementAI(MovementAI);
            ResetCooldown();
        }

        protected override void OnDeactivate()
        {
            Stand.RemoveMovementAI(MovementAI);
        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            pv_StandMovementSpeed = Stand.GetSingleStat(STAND_STAT_MOVESPEED) * 0.5f;
            pv_StandDamage = Stand.GetStat(STAND_STAT_DAMAGE) * 2;
            pv_StandKnock = Stand.GetSingleStat(STAND_STAT_KNOCKBACK) * 10f;
        }

        private void MovementAI()
        {
            if(pv_CurrentDuration <= pv_Duration / 3f)
                pv_Direction += new Vector2(0f, -1f);
            pv_StandProjectile.velocity = pv_Direction;

            foreach (NPC npc in Hooks.InGame.GetAllWithin(pv_StandProjectile, pv_StandProjectile.Center, 40f))
            {
                npc.StrikeNPC(pv_StandDamage, pv_StandKnock, pv_StandProjectile.direction, true);
            }

            pv_CurrentDuration -= FLOAT_PER_FRAME;
        }

        private float pv_StandMovementSpeed;
        private int pv_StandDamage;
        private float pv_StandKnock;
        private float pv_Duration = 0.5f;
        private float pv_CurrentDuration;
        private float pv_AttackCountdown = 0.05f;
        private float pv_AttackCurrentCountdown;
        private StarPlatinum pv_StarPlatinum;
        private Projectile pv_StandProjectile;
        private Player pv_Owner;
        private Vector2 pv_Direction;
    }
}
