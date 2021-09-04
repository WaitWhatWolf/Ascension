using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Ascension.Buffs;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;

namespace Ascension.Projectiles
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 8:52:57")]
    public sealed class Projectile_SheerHeartAttack : AscensionProjectile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.projFrames[Projectile.type] = 2;
            //Don't want it to depend on the player, the original sheer heart attack was completely independent,
            //so will it here
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = false;

            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.CountsAsHoming[Projectile.type] = true;
        }

        public override bool MinionContactDamage() => true;

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 77;
            Projectile.height = 122 / 2; //Can't be bothered to check the height, it has 2 frames and it's total is 122 so fuck it
            Projectile.tileCollide = true;

            Projectile.friendly = true;
            Projectile.minion = true;				  
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
        }

        public void Init(Stand stand)
        {
            pv_Stand = stand;

            pv_DirCountdown = new(Event_OnDirChange, 4f, Hooks.Random.Range(0, 2) == 0);
            pv_JumpHeight = pv_Stand.GetSpeed();
            pv_Speed = pv_JumpHeight / 5f;
            pv_CanSeekCountdown = new(60f / (pv_Stand.GetAttackSpeed() / 5f), true);
            
            pv_Stand.Owner.Player.AddBuff(ModContent.BuffType<Buff_SheerHeartAttack>(), 2);
            Event_OnDirChange();
        }

        public void Deinit()
        {
            pv_Stand.Owner.Player.ClearBuff(ModContent.BuffType<Buff_SheerHeartAttack>());
            Projectile.Kill();
        }

        public override void Kill(int timeLeft)
        {
            pv_TargetsHit.Clear();
        }

        public override void PostDraw(Color lightColor)
        {
            if(pv_Target != null)
            {
                ASCResources.Dusts.Dust_SheerHeartAttack_EyeLight.Create(Projectile.TopLeft + pv_EyeLocalPos);
            }
        }

        public override void AI()
        {
            if (!CheckActive(pv_Stand.Owner.Player))
                return;

            pv_DirCountdown.UpdateCountdown();

            if (pv_CanSeekTarget)
                SeekTarget();

            if (!pv_CanSeekTarget && pv_CanSeekCountdown)
                pv_CanSeekTarget = true;

            if (!pv_CanJump && pv_JumpCountdown)
                pv_CanJump = true;

            if (pv_Target != null && pv_Target.Center.Distance(Projectile.Center) < 500f)
            {
                Projectile.tileCollide = false;
                Projectile.velocity = Vector2.Zero;
                Vector2 curPos = Projectile.Center;
                Projectile.Center = Hooks.MathF.MoveTowards(curPos, pv_Target.Center, pv_Speed * 10f);

                if (Projectile.Center.Distance(pv_Target.Center) < 5f) //This whole line is here just because collision aren't working in tmodloader alpha
                {
                    pv_Target.StrikeNPC(Projectile.damage, 0f, 0);
                    int dmg = Projectile.damage;
                    float knock = 0f; bool crit = false; int dir = 0;
                    ModifyHitNPC(pv_Target, ref dmg, ref knock, ref crit, ref dir);
                }
            }
            else
            {
                Projectile.tileCollide = true;

                //Changes the speed direction based on if either the target's position relative to sheer heart attack, or pv_MoveRight
                float xVel = pv_Speed * (pv_MoveRight ? 1f : -1f);
                //SLowly applies gravity to current Y velocity
                float yVel = Projectile.velocity.Y + (pv_Stand.Owner.Player.gravity * ASCResources.FLOAT_PER_FRAME * pv_Stand.Owner.Player.gravDir * 100f);
                
                if (Hooks.InGame.BlockExistsWithin(Projectile.Center + pv_WallScanLocalPos, 3, 3, t => Main.tileSolid[t.type]) && pv_CanJump) //Checks if sheer heart attack has blocks in front of it
                {
                    yVel = -pv_JumpHeight; //Jumps if there are
                    pv_CanJump = false;
                    pv_JumpCountdown.Reset();
                }

                Projectile.velocity = new(xVel, yVel);
            }

            for(int i = 0; i < pv_TargetsHit.Count; i++)
            {
                if(pv_TargetsHit[i].Key)
                {
                    if(pv_TargetsHit[i].Value.life > 0)
                        ExplodeTarget(pv_TargetsHit[i].Value);
                    pv_TargetsHit.RemoveAt(i);
                }
            }

            //Debug.Log(Hooks.InGame.BlockExistsWithin(Projectile.Center + pv_WallScanLocalPos, 3, 4, t => Main.tileSolid[t.type]));
        }

        public override bool PreAI() => true;

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = Hooks.InGame.GetDamageWithPen(pv_Stand.GetDamage(), pv_Stand.GetArmorPen(), target);
            pv_Stand.Owner.Player.Hurt(ASCResources.DeathReasons.GetReason("SHEERHEARTATTACKDRAWBACK", this.DisplayName, pv_Stand.Owner.Player), target.damage / 4, 0);

            pv_TargetsHit.Add(new(1f, target));
            pv_Target = null;
            pv_CanSeekTarget = false;
        }

        private void ExplodeTarget(NPC target)
        {
            float dist = target.width + target.height * 3f;
            int damage = target.boss ? target.life / 10 : target.life / 2;
            var NPCs = Hooks.InGame.GetAllWithin(Projectile, target.Center, dist);
            NPCs.Remove(target);
            target.StrikeNPC(target.boss ? target.life / 20 : (target.life + target.defense), 0f, 0);
            ASCResources.Dusts.Gore_SheerHeartAttack_Explosion.Create(target.Center);
            ASCResources.Dusts.Dust_SheerHeartAttack_Explode(target).Create(target.Center);
            foreach (NPC npc in NPCs)
            {
                npc.StrikeNPC(damage, 0f, 0);
                npc.velocity = target.Center.DirectionTo(npc.Center) * Math.Min(damage / 5, 10); //Artificial knockback
            }
        }

        private void SeekTarget()
        {
            float previous = float.NegativeInfinity;
            NPC target = null;

            foreach(NPC npc in Hooks.InGame.GetAllWithin(Projectile, Projectile.Center, 2000f))
            {
                if(previous < npc.life && pv_TargetsHit.FindIndex(i => i.Value == npc) == -1)
                {
                    target = npc;
                    previous = npc.life;
                }
            }

            pv_Target = target;
        }

        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active || !pv_Stand.Active)
            {
                owner.ClearBuff(ModContent.BuffType<Buff_SheerHeartAttack>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<Buff_SheerHeartAttack>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void Event_OnDirChange()
        {
            Projectile.netUpdate = true;

            pv_MoveRight = !pv_MoveRight;
            if(pv_Target != null)
                pv_MoveRight = (pv_Target.Center.X > Projectile.Center.X);

            Projectile.spriteDirection = !pv_MoveRight ? 1 : -1;
            pv_EyeLocalPos = pv_MoveRight ? new(63, 29) : new(13, 29);
            pv_WallScanLocalPos = new Vector2(pv_MoveRight ? 56f : -24f, -16f);
            pv_DirCountdown.ForceSetCountdown(Hooks.Random.Range(4f, 8f));
        }

        private readonly List<KeyValuePair<ReturnCountdown, NPC>> pv_TargetsHit = new();

        private EventCountdown pv_DirCountdown = 10f;
        private readonly ReturnCountdown pv_JumpCountdown = 1f;
        private ReturnCountdown pv_CanSeekCountdown;
        private bool pv_MoveRight;
        private bool pv_CanSeekTarget;
        private bool pv_CanJump;
        private Stand pv_Stand;
        private NPC pv_Target;
        private Vector2 pv_WallScanLocalPos;
        private Vector2 pv_EyeLocalPos;

        private float pv_Speed;
        private float pv_JumpHeight;
    }
}
