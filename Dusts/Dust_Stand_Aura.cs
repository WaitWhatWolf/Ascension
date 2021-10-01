using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Ascension.Dusts
{
    /// <summary>
    /// An abstract class for most stand auras.
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/10/01 11:15:29")]
    public abstract class Dust_Stand_Aura : AscensionDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.customData = (0, dust.alpha);
            dust.alpha = 255;
            dust.noGravity = true;
            dust.velocity.X *= .25f;
            dust.velocity.Y = Math.Max(dust.velocity.Y.ToNegative(), -1f);
            dust.color = Hooks.Random.ChanceIn(2) ? StartColor : dust.color;
        }

        public override bool Update(Dust dust)
        {
            var data = ((int, int))dust.customData;

            if (data.Item1 > 6 && dust.alpha <= data.Item2)
            {
                dust.alpha += Hooks.Random.Range(4, 7);

                if (data.Item1 == 26)
                    dust.color = EndColor;

                if (data.Item2 == 56 || dust.alpha >= 255)
                {
                    dust.active = false;
                    return true;
                }
            }
            else
            {
                dust.alpha -= Hooks.Random.Range(1, 5);
            }

            dust.customData = (data.Item1 + 1, data.Item2);

            return true;
        }

        /// <summary>
        /// A random chance to turn a particle's starting color to this color.
        /// </summary>
        protected abstract Color StartColor { get; }
        /// <summary>
        /// Particles will go towards this color after a dozen frames.
        /// </summary>
        protected abstract Color EndColor { get; }
    }
}
