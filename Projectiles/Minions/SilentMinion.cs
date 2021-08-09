using Ascension.Attributes;
using Ascension.Enums;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Ascension.ASCResources;

namespace Ascension.Projectiles.Minions
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class SilentMinion : AscensionProjectile
    {
        [ModifiedBy(Dev.WaitWhatWolf, "Changed sub-folder value from a raw string value to a constant in Ascension.ASCResources.", 2021, 08, 09)]
        public override string Texture => ASCResources.GetAssetsPath(ItemAssetType.Projectiles, ASSETS_SUBPATH_MINIONS, this);
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Baby-Silent Killer Minion");
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 3;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // These below are needed for a minion
            // Denotes that this Projectile is a pet or minion
            Main.projPet[Projectile.type] = true;
            // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            // Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
        }
        public sealed override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 44;
            // Makes the minion go through tiles freely
            Projectile.tileCollide = false;

            // These below are needed for a minion weapon
            // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.friendly = true;
            // Only determines the damage type
            Projectile.minion = true;
            // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.minionSlots = 1f;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            Projectile.penetrate = -1;
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            counter++;
            counter2++;
            Player player = Main.player[Projectile.owner];

            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
            {
                player.ClearBuff(ModContent.BuffType<SilentMinionBuff>());
            }
            if (player.HasBuff(ModContent.BuffType<SilentMinionBuff>()))
            {
                Projectile.timeLeft = 2;
            }
            #endregion

            #region General behavior
            Vector2 idlePosition = player.Center;
            idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is Projectile.minionPos
            float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -player.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            Vector2 vectorToIdlePosition = idlePosition - Projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the Projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.04f;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                // Fix overlap with other minions
                Projectile other = Main.projectile[i];
                if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
                    else Projectile.velocity.X += overlapVelocity;

                    if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
                    else Projectile.velocity.Y += overlapVelocity;
                }
            }
            #endregion

            #region Find target
            // Starting search distance
            float distanceFromTarget = 700f;
            Vector2 targetCenter = Projectile.position;
            bool foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
                Projectile.friendly = foundTarget;
                #endregion

                #region Movement

                // Default movement parameters (here for attacking)
                float speed = 8f;
                float inertia = 20f;

                if (foundTarget)
                {
                    // Minion has a target: attack (here, fly towards the enemy)
                    if (distanceFromTarget > 40f)
                    {
                        // The immediate range around the target (so it doesn't latch onto it when close)
                        if (Main.rand.Next(25) == 0)
                        {
                            Vector2 direction = targetCenter - Projectile.Center;
                            direction.Normalize();
                            direction *= speed;
                            direction.Y = 280;
                            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia /2;
                        }
                        if (Main.rand.Next(25) == 0)
                        {
                            Vector2 direction = targetCenter - Projectile.Center;
                            direction.Normalize();
                            direction *= speed;
                            direction.Y = -280;
                            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia / 2;
                        }
                        else
                        {
                            Vector2 direction = targetCenter - Projectile.Center;
                            direction.Normalize();
                            direction *= speed;
                            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                        }
                    }
                }
                else
                {
                    // Minion doesn't have a target: return to player and idle
                    if (distanceToIdlePosition > 600f)
                    {
                        // Speed up the minion if it's away from the player
                        speed = 12f;
                        inertia = 60f;
                    }
                    else
                    {
                        // Slow down the minion if closer to the player
                        speed = 4f;
                        inertia = 80f;
                    }
                    if (distanceToIdlePosition > 20f)
                    {
                        // The immediate range around the player (when it passively floats about)

                        // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                        vectorToIdlePosition.Normalize();
                        vectorToIdlePosition *= speed;
                        Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                    }
                    else if (Projectile.velocity == Vector2.Zero)
                    {
                        // If there is a case where it's not moving at all, give it a little "poke"
                        Projectile.velocity.X = -0.15f;
                        Projectile.velocity.Y = -0.05f;
                    }
                }
                #endregion
                #region Animation and visuals
                // So it will lean slightly towards the direction it's moving
                Projectile.rotation = Projectile.velocity.X * 1f;

                // This is a simple "loop through all frames from top to bottom" animation
                if (foundTarget)
                {
                    targetFound = true;
                }
                if (!foundTarget)
                {
                    targetFound = false;
                }
                if (!targetFound)
                {
                    if (counter2 >= 0 && counter2 < 30)
                    {
                        Projectile.frame = 0;
                    }
                    if (counter2 >= 31)
                        Projectile.frame = 1;
                    if (counter2 >= 60)
                        {
                            counter2 = 0;
                        }

                }
                if (targetFound)
                {
                    Projectile.alpha = 0;
                    Projectile.frame = 2;
                }

                if (Projectile.alpha <= 210 && wait == false)
                {
                    Projectile.alpha += 10;
                    if (Projectile.alpha > 210)
                    {
                        counter = 0;
                        wait = true;

                    }
                }
                if (wait == true)
                {
                    if (counter > 90)
                    {
                        Projectile.alpha -= 10;
                        if (Projectile.alpha < 10)
                        {
                            wait = false;
                        }
                    }
                }
                #endregion
            }
        }

        bool targetFound = false;
        int counter2;
        int counter;
        bool wait = false;
    }
}
