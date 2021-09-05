using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Ascension.Buffs
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 21:46:21")]
    public sealed class Buff_BitesTheDust : AscensionBuff
    {
        protected override string DescriptionDefault => "Death is not your concern.";
        protected override bool SaveBuff => true;
        protected override bool CountsAsDebuff => true;
        protected override bool DisplayBuffTimer => false;

        public override void Update(Player player, ref int buffIndex)
        {
            var ascPlayer = player.GetModPlayer<AscendedPlayer>();
            if (((StandAbility_KillerQueen_BitesTheDust)ascPlayer.in_Stand.Abilities[3]).BuffAvailable)
                player.buffTime[buffIndex] = 60;
            else
            {
                ascPlayer.RemoveCustomDeath(Event_BitesTheDust);
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            var ascPlayer = player.GetModPlayer<AscendedPlayer>();
            if (!ascPlayer.HasCustomDeath(Event_BitesTheDust))
                ascPlayer.AddCustomDeath(Event_BitesTheDust);

            return true;
        }

        private void Event_BitesTheDust(AscendedPlayer arg1, Player arg2)
        {
            Main.TeleportEffect(Rectangle.Empty, 0);
            arg2.position = (arg2.SpawnX >= 0 && arg2.SpawnX >= 0 
                ? new Point(arg2.SpawnX, arg2.SpawnY) 
                : arg1.SpawnPos).ToWorldCoordinates();
            ASCResources.Sound.kiraYoshikageTheme.Play();
            arg2.statLife = arg2.statLifeMax;
            arg2.immuneTime = 320;
        }
    }
}
