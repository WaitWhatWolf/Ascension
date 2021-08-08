using Ascension.Players;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Ascension.Debuffs.StandUnique
{
    public sealed class SD_StarPlatinum_ORAd : StandDebuff
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

        public SD_StarPlatinum_ORAd(Stand stand, Vector2 direction, float speed, float duration = 0.5f) : base(stand)
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
