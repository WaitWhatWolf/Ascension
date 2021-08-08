using Ascension.Debuffs.StandUnique;
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
    /// <summary>
    /// Global NPC class which handles any stand interaction.
    /// </summary>
    public sealed class StandHandlerNPC : GlobalNPC
    {
        public List<StandDebuff> Debuffs { get; } = new List<StandDebuff>();

        public override bool InstancePerEntity => true;

        public void AddDebuff(StandDebuff debuff) 
        {
            debuff.Parent = this;
            debuff.Init();
            Debuffs.Add(debuff);

            Debug.Log($"{this.FullName} is now debuffed with {debuff.GetType().Name}");
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

        public override void PostAI(NPC npc)
        {
            for (int i = 0; i < Debuffs.Count; i++)
            {
                Debuffs[i].Update();
                if (Debuffs[i].StopsAI()) //this is here because for some reason AI doesn't work no matter what...
                {
                    Debug.Log(Debuffs[i]);
                    Debuffs[i].CustomAI(npc);
                }
            }
        }

    }
}
