using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

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
                ASCResources.Dusts.Gore_Stand_KillerQueen_Explosion.Create(Parent.NPC.position);
                ASCResources.Dusts.Dust_Stand_KillerQueen_Explosion.Create(Parent.NPC.position);

                foreach(NPC npc in NPCs)
                {
                    npc.StrikeNPC(Damage / 2, 0f, 0);
                    Vector2 dir = (npc.Center - Parent.NPC.Center);
                    dir.Normalize();
                    npc.velocity = dir * Knockback;
                }

                pv_Executed = true;
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

        public ReturnCountdown Countdown { get; }
        private int Damage;
        private int Pen;
        private float Knockback;
        private bool pv_Executed;
    }
}
