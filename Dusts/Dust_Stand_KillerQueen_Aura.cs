using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Ascension.Dusts
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/07 19:38:18")]
    public class Dust_Stand_KillerQueen_Aura : AscensionDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.customData = (0, dust.alpha);
            dust.alpha = 255;
            dust.noGravity = true;
            dust.velocity.X *= .25f;
            dust.velocity.Y = -1f;
        }

        public override bool Update(Dust dust)
        {
            var data = ((int, int))dust.customData;

            if (data.Item1 > 6 && dust.alpha <= data.Item2)
            {
                dust.alpha += 12;

                if (data.Item1 == 7 && Hooks.Random.Range(0, 2) == 0)
                    dust.color = Color.DodgerBlue;

                if (data.Item2 == 42 || dust.alpha >= 255)
                {
                    dust.active = false;
                    return true;
                }
            }
            else
            {
                dust.alpha -= 5;
            }

            dust.customData = (data.Item1 + 1, data.Item2);

            return true;
        }
    }
}
