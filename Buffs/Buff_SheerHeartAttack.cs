using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Ascension.Projectiles;
using Ascension.Utility;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 9:00:19")]
    public sealed class Buff_SheerHeartAttack : AscensionBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            // if the minions exist reset the buff time, otherwise remove the buff from the player.
            if (player.GetModPlayer<AscendedPlayer>().in_Stand.Abilities[2] is StandAbility_KillerQueen_SheerHeartAttack shaab && shaab.Active)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        [Note(Dev.WaitWhatWolf, "Yes, the dot at the end is necessary...I have OCD, pls help")]
        protected override string DescriptionDefault => 
            "Killer Queen" 
            + " has called "
            + "Sheer Heart Attack"
            + '.';

        protected override bool DisplayBuffTimer => false;
        protected override bool CountsAsDebuff => false;
        protected override bool SaveBuff => false;
    }
}
