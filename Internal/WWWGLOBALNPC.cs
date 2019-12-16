using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using WarWolfWorks_Mod.Buffs;

namespace WarWolfWorks_Mod.Internal
{
    public class WWWGLOBALNPC : GlobalNPC
    {
        public struct SubGlobalNPC
        {
            public WWWGLOBALNPC GlobalNPC;
            public NPC VanillaNPC;

            public SubGlobalNPC(WWWGLOBALNPC global, NPC npc)
            {
                GlobalNPC = global;
                VanillaNPC = npc;
            }
        }

        public static List<SubGlobalNPC> WWWGLOBALNPCS = new List<SubGlobalNPC>();
        protected WWWMOD WWWMOD => (WWWMOD)mod;
        public SubGlobalNPC ThisNPC { get; private set; }
        public bool IsStand { get; private set; } = false;

        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            WWWMOD.OnTimeStopped += StopTime;
            WWWMOD.OnTimeResumed += ResumeTime;
            ThisNPC = new SubGlobalNPC(this, npc);
            WWWGLOBALNPCS.Add(ThisNPC);
        }

        private int GetDebuffType(StandType of)
        {
            int type = of == StandType.STAR_PLATINUM
                ? ModContent.BuffType<StoppedStarPlatinumDebuff>()
                : ModContent.BuffType<StoppedTheWorldDebuff>();
            return type;
        }

        private Color PreviousColor { get; set; }
        private float PreviousStepSpeed { get; set; }
        private double DamageToDealAfterTime { get; set; }

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            return !WWWMOD.TimeIsStopped;
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if(WWWMOD.TimeIsStopped) DamageToDealAfterTime += damage;
        }

        private void StopTime(WWWMOD from, StandType to)
        {
            ThisNPC.VanillaNPC.AddBuff(GetDebuffType(to), int.MaxValue);
            PreviousStepSpeed = ThisNPC.VanillaNPC.stepSpeed;
            ThisNPC.VanillaNPC.stepSpeed = 0;
            ThisNPC.VanillaNPC.active = false;

            PreviousColor = ThisNPC.VanillaNPC.color;
            ThisNPC.VanillaNPC.color = new Color(Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), Main.rand.NextFloat(0f, 1f), .9f);
        }

        private void ResumeTime(WWWMOD from, StandType to)
        {
            ThisNPC.VanillaNPC.DelBuff(GetDebuffType(to));
            ThisNPC.VanillaNPC.StrikeNPCNoInteraction((int)DamageToDealAfterTime, 20, 0, true, true, true);
            DamageToDealAfterTime = 0;
            ThisNPC.VanillaNPC.stepSpeed = PreviousStepSpeed;
            ThisNPC.VanillaNPC.active = false;

            ThisNPC.VanillaNPC.color = PreviousColor;
        }
    }
}
