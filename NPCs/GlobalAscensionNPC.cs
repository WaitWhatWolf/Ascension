using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
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
