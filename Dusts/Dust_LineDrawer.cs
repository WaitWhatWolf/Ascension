using Ascension.Attributes;
using Ascension.Enums;
using Ascension.Utility;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace Ascension.Dusts
{
    /// <summary>
    ///
    /// </summary>
    [CreatedBy(Dev.WaitWhatWolf, "2021/09/04 19:29:58")]
    public class Dust_LineDrawer : AscensionDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noLightEmittence = false;
        }

        public override bool Update(Dust dust)
        {
            (Vector2, float, int, int) data = ((Vector2, float, int, int))dust.customData;
            dust.position = data.Item1;
            dust.rotation = data.Item2;
            dust.frame.Width = data.Item3;
            dust.frame.Height = data.Item4;
            return false;
        }
    }
}
