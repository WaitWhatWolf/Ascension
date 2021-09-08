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
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/08 18:48:29")]
    public class Dust_Stand_Menacing : AscensionDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.customData = dust.position;
            dust.frame.X = 0;
            dust.frame.Y = 0;
            dust.frame.Width = 36;
            dust.frame.Height = 36;
            dust.alpha = 0;
            dust.velocity /= 4;
            dust.scale = 0.75f;
            dust.velocity.Y = -0.25f;
        }

        public override bool Update(Dust dust)
        {
            Vector2 pos = ((Vector2)dust.customData) + new Vector2(Hooks.Random.Range(-dust.velocity.X, dust.velocity.X) * 2f, dust.velocity.Y.ToNegative() * 2f);
            dust.customData = pos;
            dust.position = pos + Hooks.Random.Range(new Vector2(-dust.velocity.X, -dust.velocity.Y), 
                new Vector2(dust.velocity.X, dust.velocity.Y));
            dust.scale -= (Hooks.Random.Range(-0.05f, 0.1f) * 0.2f);
            dust.alpha += Hooks.Random.Range(-2, 4);

            if (dust.scale <= 0.05f || dust.alpha >= 200)
                dust.active = false;

            return false;
        }
    }
}
