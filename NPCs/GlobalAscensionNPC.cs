using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Interfaces;
using Ascension.Internal;
using Ascension.Items;
using Ascension.Players;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 06)]
    public sealed class GlobalAscensionNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void OnKill(NPC npc)
        {
            if(npc.boss)
            {
                Main.player[Main.myPlayer].GetModPlayer<AscendedPlayer>().AddDefeatedBoss(npc.FullName);
            }
        }

    }
}
