using Ascension.Attributes;
using Ascension.Buffs.StandUnique;
using Ascension.Enums;
using Ascension.Players;
using Ascension.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.NPCs
{
    /// <summary>
    /// Global NPC class which handles any stand interaction.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 08)]
    public sealed class StandHandlerNPC : GlobalNPC
    {
        public List<StandBuff> Debuffs { get; } = new List<StandBuff>();

        public override bool InstancePerEntity => true;

        public void AddDebuff(StandBuff debuff) 
        {
            if (Debuffs.Contains(debuff))
                return;

            debuff.Parent = this;
            debuff.Init();
            Debuffs.Add(debuff);

            Debug.Log($"{this.Name} is now debuffed with {debuff.GetType().Name}");
        }

        public override bool PreAI(NPC npc)
        {
            bool toReturn = true;

            for (int i = 0; i < Debuffs.Count; i++)
            {
                if (Debuffs[i].AllowRemove())
                {
                    Debug.Log($"{npc.FullName} is no longer debuffed with {Debuffs[i].GetType().Name}");
                    
                    Debuffs.RemoveAt(i);
                    continue;
                }

                //Debug.Log($"ToReturn: {toReturn} ::: StopsAI: {Debuffs[i].StopsAI()}");
                if (toReturn && Debuffs[i].StopsAI())
                {
                    toReturn = false;
                }
            }

            return toReturn;
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            spawnRate = ASCWorld.IsInTheWorld ? 0 : spawnRate;
        }

        public override void AI(NPC npc)
        {
            for (int i = 0; i < Debuffs.Count; i++)
            {
                Debuffs[i].Update();
                if (Debuffs[i].StopsAI())
                {
                    //Debug.Log(Debuffs[i]);
                    Debuffs[i].CustomAI(npc);
                }
            }
        }

        public override bool CheckDead(NPC npc) => ASCWorld.IsInTheWorld;

        private AscensionWorld ASCWorld
        {
            get 
            { 
                if(!pv_WorldLoaded)
                {
                    pv_ASCWorld = ModContent.GetInstance<AscensionWorld>();
                    pv_WorldLoaded = true;
                }

                return pv_ASCWorld;
            }
        }
        private AscensionWorld pv_ASCWorld;
        private bool pv_WorldLoaded;
    }
}
