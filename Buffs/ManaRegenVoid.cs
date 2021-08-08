using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    class ManaRegenVoid : AscensionBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Influence");
            Description.SetDefault("Void grants you mana");
            Main.debuff[Type] = false;
        }
        public static void Effects(Player player)
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statMana += 2;
            buffIndex++;
        }
    }
}
