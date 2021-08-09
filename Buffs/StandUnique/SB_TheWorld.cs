using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.World;
using Terraria;

namespace Ascension.Buffs.StandUnique
{
    [CreatedBy(Dev.WaitWhatWolf, 2021, 08, 09)]
    public class SB_TheWorld : StandBuff
    {
        public override bool AllowRemove() => !pv_ASCWorld.IsInTheWorld;

        public override bool StopsAI() => pv_ASCWorld.IsInTheWorld;

        public override void CustomAI(NPC npc)
        {

        }

        public SB_TheWorld(AscensionWorld ascWorld, Stand stand) : base(stand)
        {
            pv_ASCWorld = ascWorld;
        }

        private AscensionWorld pv_ASCWorld;
    }
}
