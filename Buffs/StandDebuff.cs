using Terraria;
using Terraria.ModLoader;

namespace WarWolfWorks_Mod.Buffs
{
    public abstract class StandDebuff : ModBuff
    {
        protected abstract string StandDebuffName { get; }
        protected abstract string StandDebuffDescription { get; }
        public override void SetDefaults()
        {
            DisplayName.SetDefault(StandDebuffName);
            Description.SetDefault(StandDebuffDescription);
            Main.debuff[Type] = true;
            Main.buffAlpha[Type] = 0.2f;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
