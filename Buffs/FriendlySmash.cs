using Ascension.Attributes;
using Ascension.Enums;
using Terraria;

namespace Ascension.Buffs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class FriendlySmash : AscensionBuff
    {
        protected override string DisplayNameDefault { get; } = "Smashed Far";
        protected override string DescriptionDefault { get; } = "Immense power hits you";

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.boss == false)
            {
                npc.velocity.Y = npc.velocity.Y - 10;
                if (Main.rand.Next(2) == 0)
                {
                    npc.velocity.X = -npc.velocity.X * 2;
                }
                else
                {
                    npc.velocity.X = npc.velocity.X * 2;
                }
            }
        }

    }
}
