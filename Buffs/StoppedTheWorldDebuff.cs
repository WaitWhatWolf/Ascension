using Terraria;
using Terraria.ModLoader;

namespace WarWolfWorks_Mod.Buffs
{
    public class StoppedTheWorldDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("ザ・ワールド");
            Description.SetDefault("The World has stopped time!");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.persistentBuff[Type] = false;
        }
    }
}
