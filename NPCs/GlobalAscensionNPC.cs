using Ascension.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    public sealed class GlobalAscensionNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            if(npc.boss)
            {
                Main.player[Main.myPlayer].GetModPlayer<AscendedPlayer>().AddDefeatedBoss(npc.FullName);
            }
        }
    }
}
