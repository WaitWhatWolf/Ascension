using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Ascension.Buffs.StandUnique
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/24 14:25:29")]
    public class SB_KillerQueen_Bomb : StandBuff
    {
        public override bool AllowRemove() => pv_Executed;

        public override void Update()
        {
            if(Countdown && !pv_Executed)
            {
                var NPCs = Hooks.InGame.GetAllWithin(Stand.GetStandProjectile(), Parent.NPC.Center, 120f);
                NPCs.Remove(Parent.NPC);
                Parent.NPC.StrikeNPC(Hooks.InGame.GetDamageWithPen(Damage, Pen, Parent.NPC), 0f, 0);
                Parent.NPC.velocity = new(0f, -2f);
                CreateParticlesAt(Parent.NPC.Center);
                CreateParticlesAt(Parent.NPC.Left);
                CreateParticlesAt(Parent.NPC.Right);
                CreateParticlesAt(Parent.NPC.Top);
                CreateParticlesAt(Parent.NPC.Bottom);
                SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Parent.NPC.Center);
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Parent.NPC.Center);

                foreach (NPC npc in NPCs)
                {
                    npc.StrikeNPC(Hooks.InGame.GetDamageWithPen(Damage / 2, Pen, Parent.NPC), 0f, 0);
                    Vector2 dir = (npc.Center - Parent.NPC.Center);
                    dir.Normalize();
                    npc.velocity = dir * Knockback;
                }

                pv_Executed = true;
            }

            if (!pv_PlayedAnim && Countdown.GetCurrentCountdown() <= Stand.GetStandModProjectile().StandAnimator.GetAnimation(ASCResources.Animations.NAME_STAND_KILLERQUEEN_TRIGGERBOMB).TotalPlayback / 2f)
            {
                pv_PlayedAnim = true;
                Stand.StandAnimator.Speed = 1f;
                Stand.StandAnimator.Play(ASCResources.Animations.NAME_STAND_KILLERQUEEN_TRIGGERBOMB);
            }
        }

        public override bool StopsAI() => false;

        public SB_KillerQueen_Bomb(Stand stand, ReturnCountdown countdown, int damage, int pen, float knockback) : base(stand)
        {
            Countdown = new(countdown.Countdown, false);
            Damage = damage;
            Pen = pen;
            Knockback = knockback;
        }

        public override void OnDeath()
        {
            //prevents killer queen from having a sped-up idle animation
            Stand.StandAnimator.Speed = 1f;
        }

        public ReturnCountdown Countdown { get; }

        private void CreateParticlesAt(Vector2 position)
        {
            ASCResources.Dusts.Gore_Stand_KillerQueen_Explosion.Create(position);
            ASCResources.Dusts.Dust_Stand_KillerQueen_Explosion.Create(position);
        }

        private bool pv_PlayedAnim;
        private int Damage;
        private int Pen;
        private float Knockback;
        private bool pv_Executed;
    }
}
