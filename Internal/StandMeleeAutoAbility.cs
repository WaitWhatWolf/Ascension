using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Utilities;
using static WarWolfWorks_Mod.Internal.Constants;

namespace WarWolfWorks_Mod.Internal
{
    /// <summary>
    /// The "Melee" <see cref="AutoAbility"/> of a stand; Make the <see cref="Stand.Projectile"/> move towards the nearest enemy and attack it. 
    /// </summary>
    public sealed class StandMeleeAutoAbility : AutoAbility
    {
        private NPC Closest;

        private bool ClosestWithinRange;

        /// <summary>
        /// Returns true if an NPC is within the stand's melee range.
        /// </summary>
        /// <returns></returns>
        public override bool Activates() => ClosestWithinRange && CooldownUp;

        private Vector2 Destination;
        private readonly Vector2 IdleDistance = new Vector2(-1, 1);
        private readonly Vector2 IdleDistanceFlipped = new Vector2(-1, 1);
        private const float STAND_MOVE_SPEED = 1f,
            STAND_MELEE_RANGE = 1f;

        private Animation AttackAnimation, IdleAnimation;

        public override Texture2D UITexture { get; }

        private Animation GetQueuedAnimation()
        {
            if (ClosestWithinRange)
                return AttackAnimation;
            else return IdleAnimation;
        }

        /// <summary>
        /// Called every in-game frame, while setting all variables and values to make this ability function.
        /// </summary>
        protected override void OnUpdate()
        {
            /*
            Closest = NPCUtils.SearchForTarget(WWWPlayer.Instance.player.position, NPCUtils.TargetSearchFlag.NPCs).NearestNPC;
            Destination = Closest != null 
                ? Closest.position 
                : WWWPlayer.Instance.player.oldPosition.X < WWWPlayer.Instance.player.position.X 
                ? WWWPlayer.Instance.player.position + IdleDistance 
                : WWWPlayer.Instance.player.position + IdleDistanceFlipped;

            WWWPlayer.Instance.Stand.Projectile.projectile.velocity = Vector2.SmoothStep(WWWPlayer.Instance.Stand.Projectile.projectile.position, Destination, STAND_MOVE_SPEED);

            ClosestWithinRange = Vector2.Distance(Closest.position, WWWPlayer.Instance.Stand.Projectile.projectile.position) <= STAND_MELEE_RANGE;

            WWWPlayer.Instance.Stand.Projectile.SetAnimation(GetQueuedAnimation());
            */

            if (WWWMOD.Instance.Key_Ability_Norm.JustPressed)
                Countdown = new TimeSpan(0, 0, 5);
        }

        /// <summary>
        /// Initiates this class using the <see cref="StandAbility"/> constructor.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cooldown">Attack speed of the stand.</param>
        /// <param name="idletxt">Animation to play when the stand is not attacking.</param>
        /// <param name="atktxt">Animation to play the the stand is attacking.</param>
        public StandMeleeAutoAbility(TimeSpan cooldown, Animation idletxt, Animation atktxt) : base()
        {
            UITexture = TEX_UI_STAND_SP_AUTO;
            Cooldown = cooldown;
            IdleAnimation = idletxt;
            AttackAnimation = atktxt;
        }
    }
}
