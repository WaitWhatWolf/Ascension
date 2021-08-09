﻿using Ascension.Buffs.StandUnique;
using Ascension.NPCs;
using Ascension.Projectiles;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using static Ascension.ASCResources.Stats;
using static Ascension.ASCResources;
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

        public override string Description => $"Star Platinum quicky rushes to it's stand user\nand punches any nearby enemy with massive damage & knockback.\nCooldown: {GetCooldown()}";

        protected override float Cooldown { get; } = 10f;

        public override Asset<Texture2D> Icon => ASCResources.Textures.Stand_Ability_StarPlatinum_ORA;

        protected override bool ActivateCondition() => CooldownReady;

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
                List<NPC> npcs = Hooks.InGame.GetAllWithin(projectile, projectile.Center, 500f);
                foreach (NPC npc in npcs)
                {
                    float knock = Stand.GetSingleStat(STAND_STAT_KNOCKBACK) * 20f;
                    npc.StrikeNPC(Stand.GetStat(STAND_STAT_DAMAGE) * 10, knock, -npc.direction);

                    //npc.GetGlobalNPC<StandHandlerNPC>(true).AddDebuff(new SD_StarPlatinum_ORAd(Stand, npc.DirectionFrom(Stand.Owner.Player.position/*projectile.Center*/), 100f));
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
            float speed = currentDist > 100f ? pv_StandMoveSpeed * (currentDist / 10f) : pv_StandMoveSpeed; //Accelerates based on distance
            Vector2 toUse = Hooks.MathF.MoveTowards(pv_StandProjectile.Center, pv_Owner.Center, pv_StandMoveSpeed * FLOAT_PER_FRAME);
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
