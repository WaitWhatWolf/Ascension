using Ascension.Attributes;
using Ascension.Buffs.StandUnique;
using Ascension.Enums;
using Ascension.Interfaces;
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
        public List<StandBuff> Buffs { get; } = new List<StandBuff>();

        public override bool InstancePerEntity => true;

        public NPC NPC { get; private set; }

        public void AddDebuff(StandBuff buff) 
        {
            if (Buffs.Contains(buff))
                return;

            buff.Parent = this;
            buff.Init();
            Buffs.Add(buff);

            if(buff is IDebuggable)
                Debug.Log($"{this.Name} is now debuffed with {buff.GetType().Name}");
        }

        public override bool PreAI(NPC npc)
        {
            bool toReturn = true;

            NPC = npc;

            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].AllowRemove())
                {
                    if(Buffs[i] is IDebuggable)
                        Debug.Log($"{npc.FullName} is no longer debuffed with {Buffs[i].GetType().Name}");
                    
                    Buffs.RemoveAt(i);
                    continue;
                }

                //Debug.Log($"ToReturn: {toReturn} ::: StopsAI: {Debuffs[i].StopsAI()}");
                if (toReturn && Buffs[i].StopsAI())
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
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].Update();
                if (Buffs[i].StopsAI())
                {
                    //Debug.Log(Debuffs[i]);
                    Buffs[i].CustomAI(npc);
                }
            }
        }

        public override void OnKill(NPC npc)
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                Buffs[i].OnDeath();
                Buffs.RemoveAt(i);
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
