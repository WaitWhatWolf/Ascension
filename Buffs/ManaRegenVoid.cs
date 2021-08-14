using Ascension.Attributes;
using Ascension.Enums;
using Terraria;

namespace Ascension.Buffs
{
    [CreatedBy(Dev.Adragon, 2021, 08, 08)]
    class ManaRegenVoid : AscensionBuff
    {
        protected override string DisplayNameDefault { get; } = "Void Influence";
        protected override string DescriptionDefault { get; } = "The Void grants you mana";
        protected override bool CountsAsDebuff { get; } = false;
        
        public override void Update(Player player, ref int buffIndex)
        {
            player.statMana += 2;
            buffIndex++;
        }
    }
}
