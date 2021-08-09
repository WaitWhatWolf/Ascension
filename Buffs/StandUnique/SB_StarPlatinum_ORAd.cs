using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Ascension.Buffs.StandUnique
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08), Obsolete("No longer used; Standard knockback is used for now.")]
    public sealed class SB_StarPlatinum_ORAd : StandBuff
    {
        public override bool AllowRemove() => pv_Cooldown <= 0;

        public override bool StopsAI() => true;

        public override void CustomAI(NPC npc)
        {
            Vector2 pos = npc.position;
            pos = Hooks.MathF.MoveTowards(pos, pos + pv_Direction, pv_Speed * ASCResources.FLOAT_PER_FRAME);
            npc.position = pos;

            pv_Cooldown -= ASCResources.FLOAT_PER_FRAME;
        }

        public SB_StarPlatinum_ORAd(Stand stand, Vector2 direction, float speed, float duration = 0.5f) : base(stand)
        {
            pv_Direction = direction * 10f;
            pv_Speed = speed;
            pv_Cooldown = duration;
        }

        private Vector2 pv_Direction;
        private float pv_Speed;
        private float pv_Cooldown;
    }
}
