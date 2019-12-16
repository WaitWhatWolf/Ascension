using Terraria;
using Terraria.ModLoader;

namespace WarWolfWorks_Mod.Buffs
{
    public class StoppedStarPlatinumDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("ザ・ワールド");
            Description.SetDefault("Star Platinum has stopped time!");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.persistentBuff[Type] = false;
        }
    }
}
