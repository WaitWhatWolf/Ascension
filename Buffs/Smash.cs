using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    class Smash : AscensionBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smashed far");
            Description.SetDefault("Immense power hits you");
            Main.debuff[Type] = true;
        }
        public static void Effects(Player player)
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.Y = player.velocity.Y - 20;
            if (Main.rand.Next(2) == 0)
            {
                player.velocity.X = -player.velocity.X * 2;
            }
            else
            {
                player.velocity.X = player.velocity.X * 2;
            }

            buffIndex++;
        }
    }
}
