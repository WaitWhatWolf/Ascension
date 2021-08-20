using Ascension.Attributes;
using Ascension.Enums;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/14 17:35:37")]
    public class GNPC_ParasitesHandler : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void PostAI(NPC npc)
        {
            if (pv_ParasiteCount <= 0)
                return;
            pv_Countdown -= ASCResources.FLOAT_PER_FRAME;

            if(pv_Countdown <= 0)
            {
                pv_Countdown = 0.2f;
                int damage = 3;
                damage += Math.Min(npc.defense, 2);
                for(int i = 0; i < pv_ParasiteCount; i++)
                    npc.StrikeNPC(damage, 0, 0);
                pv_CountRemover++;
                if(pv_CountRemover >= 4)
                {
                    pv_CountRemover = 0;
                    pv_ParasiteCount--;
                }
            }
        }

        public void UpdateParasites()
        {
            if (pv_ParasiteCount <= 0)
                pv_ParasiteCount = 1;
        }

        public void AddParasite()
        {
            pv_ParasiteCount++;
            pv_CountRemover = 0;
        }

        private int pv_ParasiteCount;
        private float pv_Countdown;
        private int pv_CountRemover;
    }
}
