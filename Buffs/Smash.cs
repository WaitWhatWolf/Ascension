using Ascension.Attributes;
using Ascension.Enums;
using Terraria;

namespace Ascension.Buffs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class Smash : AscensionBuff
    {
        protected override string DisplayNameDefault { get; } = "Smashed far";
        protected override bool CountsAsDebuff { get; } = true;
        protected override string DescriptionDefault { get; } = "Immense power hits you";

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
