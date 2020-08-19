using Microsoft.Xna.Framework;
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

        public override string Texture => TN_STAND_SP_AB_MAIN;

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
            Closest = NPCUtils.SearchForTarget(Owner.player.position, NPCUtils.TargetSearchFlag.NPCs).NearestNPC;
            Destination = Closest != null 
                ? Closest.position 
                : Owner.player.oldPosition.X < Owner.player.position.X 
                ? Owner.player.position + IdleDistance 
                : Owner.player.position + IdleDistanceFlipped;

            Owner.Stand.Projectile.projectile.velocity = Vector2.SmoothStep(Owner.Stand.Projectile.projectile.position, Destination, STAND_MOVE_SPEED);

            ClosestWithinRange = Vector2.Distance(Closest.position, Owner.Stand.Projectile.projectile.position) <= STAND_MELEE_RANGE;

            Owner.Stand.Projectile.SetAnimation(GetQueuedAnimation());
        }

        /// <summary>
        /// Initiates this class using the <see cref="StandAbility"/> constructor.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cooldown">Attack speed of the stand.</param>
        /// <param name="idletxt">Animation to play when the stand is not attacking.</param>
        /// <param name="atktxt">Animation to play the the stand is attacking.</param>
        public StandMeleeAutoAbility(WWWPlayer player, TimeSpan cooldown, Animation idletxt, Animation atktxt) : base(player)
        {
            Cooldown = cooldown;
            IdleAnimation = idletxt;
            AttackAnimation = atktxt;
        }
    }
}
