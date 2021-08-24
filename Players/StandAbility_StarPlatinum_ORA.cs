using Ascension.Buffs.StandUnique;
using Ascension.NPCs;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using static Ascension.ASCResources.Stats;
using static Ascension.ASCResources;
using static Ascension.ASCResources.Textures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Ascension.Attributes;
using Ascension.Enums;

namespace Ascension.Players
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public class StandAbility_StarPlatinum_ORA : StandAbility
    {
        public override string Name { get; } = "ORA!!!";

        public override string Description => Hooks.Colors.GetColoredTooltipText("Star Platinum", Hooks.Colors.Tooltip_Stand_Title) + " quicky rushes to it's stand user\nand punches any nearby enemy with massive damage & knockback.\n"
            + Hooks.Colors.GetColoredTooltipText($"Cooldown: {GetCooldown()}", Hooks.Colors.Tooltip_Stand_Ability_Cooldown);

        protected override ReturnCountdown Countdown { get; } = 10f;

        public override Asset<Texture2D> Icon => GetTexture(STAND_ABILITY_STARPLATINUM_ABILITY1);

        protected override bool ActivateCondition() => CountdownReady;

        protected override bool DeactivateCondition() => pv_DeactivateReady;

        protected override void OnActivate()
        {
            pv_StarPlatinum = Stand.GetStandModProjectile() as StarPlatinum;
            pv_StandProjectile = Stand.GetStandProjectile();
            pv_Owner = Stand.Owner.Player;

            Stand.AddMovementAI(MovementAI);
            ResetCooldown();
        }

        protected override void OnDeactivate() 
        { 
            pv_DeactivateReady = false;
            Stand.RemoveMovementAI(MovementAI);
        }

        public override void Update()
        {
            base.Update();

            if (pv_WithinORARange)
            {
                Projectile projectile = Stand.GetStandProjectile();
                List<NPC> npcs = Hooks.InGame.GetAllWithin(projectile, projectile.Center, 250f);
                foreach (NPC npc in npcs)
                {
                    float knock = Stand.GetSingleStat(STAND_STAT_KNOCKBACK);
                    npc.StrikeNPC(Stand.GetStat(STAND_STAT_DAMAGE) * 10, knock, -npc.direction);
                    Vector2 direction = npc.Center - projectile.Center;
                    direction.Normalize();
                    direction -= new Vector2(0f, 0.2f); //Makes enemies tend to get knocked upwards
                    direction *= knock * 10f;
                    npc.velocity = direction;
                }

                pv_WithinORARange = false;
                pv_DeactivateReady = true;
            }
        }

        protected override void Event_OnNewBossDefeated(string obj)
        {
            //Multiplied cause it will use a reduced float based on frame speed; Basically unity's version of Time.deltaTime.
            pv_StandMoveSpeed = Stand.GetSingleStat(STAND_STAT_MOVESPEED) * 40f;
        }

        private void MovementAI()
        {
            pv_StandProjectile.velocity = Vector2.Zero;
            float currentDist = Vector2.Distance(pv_StandProjectile.Center, pv_Owner.Center);
            float speed = pv_StandMoveSpeed; //Accelerates based on distance
            if (currentDist >= 1000f) speed *= (currentDist / 10f);
            Vector2 toUse = Hooks.MathF.MoveTowards(pv_StandProjectile.Center, pv_Owner.Center, speed * FLOAT_PER_FRAME);
            pv_StandProjectile.Center = toUse;

            Vector2 directionToOwner = pv_Owner.Center - pv_StandProjectile.Center;
            directionToOwner.Normalize();

            pv_StandProjectile.direction = directionToOwner.X > 0f ? 1 : -1;

            pv_WithinORARange = currentDist < 5f;
        }

        //Required
        public StandAbility_StarPlatinum_ORA(Stand stand) : base(stand)
        {

        }

        private float pv_StandMoveSpeed;
        private bool pv_DeactivateReady;
        private bool pv_WithinORARange;
        private StarPlatinum pv_StarPlatinum;
        private Projectile pv_StandProjectile;
        private Player pv_Owner;
    }
}
