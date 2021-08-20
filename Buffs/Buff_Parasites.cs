using Ascension.Attributes;
using Ascension.Enums;
using Ascension.NPCs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/08/14 16:54:18")]
    public class Buff_Parasites : AscensionBuff
    {
        protected override string DescriptionDefault { get; } = "Parasites are eating you from the inside!";
        protected override bool CountsAsDebuff => true;
        protected override bool DisplayBuffTimer => true;

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<GNPC_ParasitesHandler>().UpdateParasites();
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            npc.GetGlobalNPC<GNPC_ParasitesHandler>().AddParasite();
            return true;
        }
    }
}
